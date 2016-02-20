using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ChGK.Core.Services.Deserialization;
using HtmlAgilityPack;

namespace ChGK.Core.DbChGKInfo
{
    internal class XmlDeserializer<T> : IDeserializer<T>
    {
        private readonly XmlSerializer _serializer;

        public XmlDeserializer()
        {
            _serializer = new XmlSerializer(typeof (T));
        }

        public Task<T> Deserialize(string responseBody)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var read = new StringReader(responseBody))
                {
                    using (var reader = XmlReader.Create(read))
                    {
                        return (T) _serializer.Deserialize(reader);
                    }
                }
            });
        }
    }

    internal interface IHtmlDeserializable<out T>
    {
        bool RecognitionPattern(HtmlNode node);

        T LoadFrom(HtmlNode node);
    }

    internal class HtmlDeserializer<T> : IDeserializer<T> where T : IHtmlDeserializable<T>, new()
    {
        public Task<T> Deserialize(string responseBody)
        {
            return Task.Factory.StartNew(() =>
            {
                var document = new HtmlDocument();
                document.LoadHtml(responseBody);

                var a = document.DocumentNode.Descendants()
                    .FirstOrDefault(node => new T().RecognitionPattern(node));

                if (a == null)
                {
                    throw new FormatException("Кажется, изменился формат ответа сервера.");
                }

                return new T().LoadFrom(a);
            });
        }
    }
}