using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace MusikApp
{
    public class FunctionsMusic
    {
        private static string api = "943162ae4e5746e913febfbdde8b0659";
        private static string youtubeapi = "AIzaSyA46C-9etYFK-CYL3shr9j5T-ZSoJmWEUc";

        public static string Econde(string text)
        {
            text = text.Replace(" ", "+");
            text = text.Replace("-", "+");
            text = text.Replace("%252c", ",");
            text = text.Replace("%2c", ",");
            text = text.Replace("%2527", "'");
            text = text.Replace("%26", "and");
            text = text.Replace("&", "and");

            return text;
        }

        public static string Decode(string text)
        {
            text = text.Replace(":", " ");
            text = text.Replace("+", " ");
            text = text.Replace("-", " ");
            text = text.Replace("/", " - ");
            text = text.Replace("%20", " ");

            return text;
        }

        public static string GetResponse(string url)
        {
            //System.Diagnostics.Debug.WriteLine(url);
            string ch = "";
            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(url);
            var response = rq.GetResponse() as HttpWebResponse;
            if (response != null)
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                ch = reader.ReadToEnd();
            }
            return ch;
        }
        public static string SearchLastFm(string query)
        {
            
            string url = "http://ws.audioscrobbler.com/2.0/?method=track.search&track="+query+"&api_key="+api+
                            "&format=json&limit=5&autocorrect=1";
            string ch = GetResponse(url);
            ch = ch.Replace("#text", "text");
            ch = ch.Replace(":totalResults", "");
            return ch;
        }


        public static string SearchArtist(string query)
        {
            string[] temp = query.Split('-');
            query = temp[0];
            //query = Econde(query);
            string url = "http://ws.audioscrobbler.com/2.0/?method=artist.search&artist="+query+
                            "&api_key=" + api + "&format=json";
            string ch = GetResponse(url);
            ch = ch.Replace("#text", "text");
            ch = ch.Replace(":totalResults", "");
            ch = ch.Replace("'", " ");
            return ch;
        }


        public static string SearchYoutube(string query)
        {

            query = Econde(query);
            //string url = "http://gdata.youtube.com/feeds/api/videos?q=" + query +
            //            "&format=5&max-results=1&start-index=1&v=2&alt=jsonc&key="+youtubeapi;

            //string url = "https://www.googleapis.com/youtube/v3/videos?q=" + query +
            //            "&format=5&max-results=1&start-index=1&v=2&alt=jsonc&key=" + youtubeapi;


            string url = "https://www.googleapis.com/youtube/v3/search?q=" + query + "&part=snippet" + "&key=" + youtubeapi;
            string ch = GetResponse(url);
            return ch;
        }

        public static string GetSimilar(string artist, string track)
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=track.getsimilar&artist="+artist+"&track=" + track + "&api_key=" + api +
                            "&format=json&limit=20&autocorrect=1";
            string ch = GetResponse(url);
            ch = ch.Replace("#text", "text");
            ch = ch.Replace(":totalResults", "");
            return ch;
        }


        public static string GetTrackInfo(string artist, string track)
        {
            artist = Econde(artist);
            track = Econde(track);
            string url = "http://ws.audioscrobbler.com/2.0/?method=track.getInfo&artist=" + artist + "&track=" + track + "&api_key=" + api +
                            "&format=json";
            string ch = GetResponse(url);
            ch = ch.Replace("#text", "text");
            ch = ch.Replace(":totalResults", "");
            return ch;
        }

        public static string GetArtistInfo(string artist)
        {
            artist = Econde(artist);
            string url = "http://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist="+ artist +
                         "&api_key="+api+"&autocorrect=1&format=json&lang=en";
            string ch = GetResponse(url);
            ch = ch.Replace("#text", "text");
            ch = ch.Replace(":totalResults", "");
            return ch;
        }

        public static string GetAddons()
        {
            string url = "http://api.andthemusic.net/music/addons";
            string ch = GetResponse(url);
            ch = ch.Replace("#text", "text");
            ch = ch.Replace(":totalResults", "");
            return ch;
        }

        public static string GetAlbums(string artist)
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=artist.gettopalbums&artist=" + artist +
                         "&api_key=" + api + "&format=json";
            string ch = GetResponse(url);
            ch = ch.Replace("#text", "text");
            ch = ch.Replace(":totalResults", "");
            return ch;
        }

        public static string GetEvents(string artist)
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=artist.getevents&artist=" + artist +
                         "&api_key=" + api + "&format=json";
            string ch = GetResponse(url);
            ch = ch.Replace("#text", "text");
            ch = ch.Replace(":totalResults", "");
            ch = ch.Replace("geo:point", "geo");
            ch = ch.Replace("geo:lat", "lat");
            ch = ch.Replace("geo:long", "long");
            return ch;
        }

        public static string GetTracksAlbums(string album, string artist)
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=album.getinfo&artist=" + artist +
                         "&api_key=" + api + "&album=" + album + "&format=json&autocorrect=1";
            string ch = GetResponse(url);
            ch = ch.Replace("#text", "text");
            ch = ch.Replace(":totalResults", "");
            return ch;
        }

        public static string GetTags()
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=tag.getTopTags&api_key=" + 
                api + "&format=json";
            string ch = GetResponse(url);
            ch = ch.Replace("#text", "text");
            ch = ch.Replace(":totalResults", "");
            return ch;
        }

        public static string GetTopTracks(string artist = null)
        {
            string url;
            if (artist != null)
            {
                artist = Econde(artist);
                url = "http://ws.audioscrobbler.com/2.0/?method=artist.gettoptracks&artist="+
                                        artist+"&api_key="+api+"&autocorrect=1&format=json";	
            }
            else
            {
                url 	= "http://ws.audioscrobbler.com/2.0/?method=chart.gettoptracks&api_key="+api+"&format=json";	
            }
            string ch = GetResponse(url);
            ch = ch.Replace("toptracks", "tracks");
            ch = ch.Replace("#text", "text");
            ch = ch.Replace(":totalResults", "");
            return ch;
        }

        public static string GetTopTags(string tag)
        {
            tag = Econde(tag);
            string url 	= "http://ws.audioscrobbler.com/2.0/?method=tag.gettoptracks&tag="+
                            tag+"&api_key="+api+"&format=json&limit=20";	
            string ch = GetResponse(url);
            ch = ch.Replace("#text", "text");
            ch = ch.Replace(":totalResults", "");
            System.Diagnostics.Debug.WriteLine(ch);
            return ch;
        }

        public static string GetTopArtist()
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=chart.gettopartists&api_key="+api+"&format=json";
            string ch = GetResponse(url);
            ch = ch.Replace("topartists", "artists");
            ch = ch.Replace("#text", "text");
            ch = ch.Replace(":totalResults", "");
            return ch;
        }

        public static string GetLyric(string artist, string track)
        {
            /*artist = Decode(artist);
            track = Decode(track);*/

            artist = Econde(artist);
            track = Econde(track);
            artist=artist.Replace("+", "-");
            track = track.Replace("+", "-");
            string url = "http://www.songlyrics.com/" + artist.Trim('-').ToLower() + "/" + track.Trim('-').ToLower() + "-lyrics/";
            //string ch = GetResponse(url);
            
            //Match m = Regex.Match(ch, "(?is)<div id=\"songLyricsDiv\"[^>]*>(.*)</div>");
            
           
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);

            var element = doc.DocumentNode.SelectSingleNode("//p[@id='songLyricsDiv']");
            if (element != null)
            {
                var itemList =
                doc.DocumentNode.SelectSingleNode("//p[@id='songLyricsDiv']").InnerHtml;
                System.Diagnostics.Debug.WriteLine(itemList);

                var lyric = new
                {
                    lyric = itemList,
                    artist = artist.Replace("-"," "),
                    track = track.Replace("-", " ")
                };
                return Json.Encode(lyric);
            }
            else
            {
                var lyric = new
                {
                    lyric = "",
                    artist = artist,
                    track = track
                };
                return Json.Encode(lyric);
            }
            
        }

        public static string GetNewReleases()
        {
            string url 	= "https://itunes.apple.com/WebObjects/MZStore.woa/wpa/MRSS/newreleases/sf=143441/limit=5/rss.xml";
	        string ch = GetResponse(url);
            ch = ch.Replace("itms:", "");
            return ch;
        }

        public static string GetTopSongsItunes()
        {
            string url = "https://itunes.apple.com/us/rss/topsongs/limit=5/json";
            string ch = GetResponse(url);
            ch = ch.Replace("itms:", "");
            ch = ch.Replace("im:", "");
            return ch;
        }

        public static int GetGoogleLinks(string query)
        {
            string url = "http://www.google.com/search?q=site%3A"+query;
            string ch = GetResponse(url);
            if (ch.Length == 0) return 0;
            if (ch.IndexOf("results", StringComparison.Ordinal)<0) return 0;
            string matchexpr = "/About (.*?) results/sim";
            Match m = Regex.Match(ch, matchexpr);
            if (m.Length == 0) return 0;
            return Convert.ToInt32(m.Value.Replace(",",""));
        }

    }
}