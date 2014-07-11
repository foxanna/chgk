using System;
using System.Xml.Serialization;
using System.Linq;
using System.Collections.Generic;
using ChGK.Core.Models;

namespace ChGK.Core.DbChGKInfo.Dto
{
	public class LastAddedTournamentsDto : IHtmlDeserializable<LastAddedTournamentsDto>
	{
		public List<ITournament> Tournaments { get; private set; }

		public bool RecognitionPattern (HtmlAgilityPack.HtmlNode node)
		{
			return node.GetAttributeValue ("class", string.Empty).Equals ("last_packages");
		}

		public LastAddedTournamentsDto LoadFrom (HtmlAgilityPack.HtmlNode node)
		{
			try {
				Tournaments = node.Elements ("tr").Skip (1).Select (tr => {
					var tds = tr.Elements ("td").ToList ();

					var tournament = new Tournament {
						FileName = tds [0].FirstChild.Attributes.FirstOrDefault (attr => attr.OriginalName.Equals ("href")).Value,
						Name = System.Net.WebUtility.HtmlDecode (tds [0].FirstChild.InnerText.Trim ()),
						PlayedAt = tds [0].LastChild.InnerText.Trim (),
						AddedAt = tds [2].InnerText.Trim (),
						Tours = tds [1].Elements ("a").Select (a => new Tour () {
							FileName = a.Attributes.FirstOrDefault (attr => attr.OriginalName.Equals ("href")).Value,
							Name = System.Net.WebUtility.HtmlDecode (a.InnerText.Trim ()),
						}).Cast<ITour> ().ToList (),
					};

					return tournament;
				}).Cast<ITournament> ().ToList ();
			} catch (Exception e) {
				throw new Exception ("Oops, ошибочка при парсинге: \n" + e.Message);
			}

			return this;
		}
	}
}

