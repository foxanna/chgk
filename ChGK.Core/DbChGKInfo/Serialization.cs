using System;
using ChGK.Core.NetworkService;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Linq;
using HtmlAgilityPack;

namespace ChGK.Core.DbChGKInfo
{
	class XmlDeserializer<T> : IDeserializer<T>
	{
		readonly XmlSerializer serializer;

		public XmlDeserializer ()
		{
			serializer = new XmlSerializer (typeof(T));
		}

		public Task<T> Deserialize (string responseBody)
		{
			return Task.Factory.StartNew<T> (() => {
				using (System.IO.StringReader read = new StringReader (responseBody)) {
					using (XmlReader reader = XmlReader.Create (read)) {
						return  (T)serializer.Deserialize (reader);
					}
				}
			});
		}
	}

	interface IHtmlDeserializable<T>
	{
		bool RecognitionPattern (HtmlNode node);

		T LoadFrom (HtmlNode node);
	}

	class HtmlDeserializer<T> : IDeserializer<T> where T : IHtmlDeserializable<T>, new()
	{
		public Task<T> Deserialize (string responseBody)
		{
			return Task.Factory.StartNew<T> (() => {
				var document = new HtmlDocument ();
				document.LoadHtml (responseBody);

				var a = document.DocumentNode.Descendants ()
					.FirstOrDefault (node => new T ().RecognitionPattern (node));

				if (a == null) {
					throw new FormatException ("Кажется, изменился формат ответа сервера.");
				}

				return new T ().LoadFrom (a);
			});
		}
	}

}

