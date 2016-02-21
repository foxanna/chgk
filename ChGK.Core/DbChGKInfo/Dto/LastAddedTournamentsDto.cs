using System;
using System.Collections.Generic;
using System.Linq;
using ChGK.Core.Models;
using HtmlAgilityPack;
using PCLWebUtility;

namespace ChGK.Core.DbChGKInfo.Dto
{
    public class LastAddedTournamentsDto : IHtmlDeserializable<LastAddedTournamentsDto>
    {
        public List<ITournament> Tournaments { get; private set; }

        public bool RecognitionPattern(HtmlNode node)
        {
            return node.GetAttributeValue("class", string.Empty).Equals("last_packages");
        }

        public LastAddedTournamentsDto LoadFrom(HtmlNode node)
        {
            try
            {
                Tournaments = node.Elements("tr").Skip(1).Select(tr =>
                {
                    var tds = tr.Elements("td").ToList();

                    var tournament = new Tournament
                    {
                        Id = tds[0].FirstChild.Attributes.FirstOrDefault(attr => attr.OriginalName.Equals("href")).Value,
                        Name = WebUtility.HtmlDecode(tds[0].FirstChild.InnerText.Trim()),
                        PlayedAt = tds[0].LastChild.InnerText.Trim(),
                        AddedAt = tds[2].InnerText.Trim(),
                        Tours = tds[1].Elements("a").Select(a => new TourDto
                        {
                            FileName = a.Attributes.FirstOrDefault(attr => attr.OriginalName.Equals("href")).Value,
                            Name = WebUtility.HtmlDecode(a.InnerText.Trim())
                        }.ToModel()).ToList()
                    };

                    return tournament;
                }).Cast<ITournament>().ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Oops, ошибочка при парсинге: \n" + e.Message);
            }

            return this;
        }
    }
}