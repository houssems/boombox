using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Twilio;

namespace MusikApp.Controllers
{
    public class musicController : Controller
    {

        public ActionResult Sms(string to, string artist, string track, string mp3)
        {
            var twilio = new TwilioRestClient("AC454e8d689872f81ee265b2b283ac0de9", "057ea936892f76a188b7896215f9f378");
            var content = "\nEnjoy listening to " + artist + " - " + track +
                     "\n\n\nto download mp3 please folow this link : " + mp3 + "\n\n\nViSiT MuSiC IsSaTsO ;-)\nhttp://houssemdh-001-site1.mywindowshosting.com/";
            var msg = twilio.SendMessage("+18707255547", to, content);
            //System.Diagnostics.Debug.WriteLine(msg);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendSms(string to, string artist, string track, string mp3)
        {
            string url = "https://secure.hoiio.com/open/sms/send";
            string app_id = "?app_id=IOuDLncfQAssY2pW";
            string token = "&access_token=YZ4jJa6Iu5bZvYFy";
            string sn = "&dest="+to;
            string sender = "&sender_name=MusicIssatso";

            var content = "\nEnjoy listening to " + artist + " - " + track +
                     "\n\n\nto download mp3 please folow this link : " + mp3 + "\n\n\nViSiT MuSiC IsSaTsO ;-)\nhttp://houssemdh-001-site1.mywindowshosting.com/";
            string msg = "&msg=" + content;
            string res = FunctionsMusic.GetResponse(url + app_id + token + sn + msg);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Call(string to, string artist, string track, string mp3)
        {
            string url = "https://secure.hoiio.com/open/ivr/start/dial";
            string app_id = "?app_id=IOuDLncfQAssY2pW";
            string token = "&access_token=YZ4jJa6Iu5bZvYFy";
            string sn = "&dest=" + to;
            string sender = "&sender_name=MusicIssatso";

            var content = "\nEnjoy listening to " + artist + " - " + track +
                     "\n\n\nto download mp3 please folow this link : " + mp3 + "\n\n\nViSiT MuSiC IsSaTsO ;-)\nhttp://houssemdh-001-site1.mywindowshosting.com/";
            string msg = "&msg=" + content;
            string res = FunctionsMusic.GetResponse(url + app_id + token + sn + msg);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /music/
        public ActionResult Typeahead(string query = "akon")
        {
            dynamic c = JObject.Parse(FunctionsMusic.SearchLastFm(query));
            int x = 0;
            JArray tab = new JArray();
            foreach (var value in c.results.trackmatches.track)
            {
                
                Console.WriteLine(value.name);
                if (value.image != null)
                {
                    JObject artist = new JObject(
                    new JProperty("image", value.image[0].text),
                    new JProperty("artist", value.artist),
                    new JProperty("name", value.name),
                    new JProperty("value", value.artist + "-" + value.name));


                    tab.Add(artist);
                    x++;
                }
                
            }
            //return Json(JsonConvert.SerializeObject(tab), "json", JsonRequestBehavior.AllowGet);
            return Content(tab.ToString(), "application/json");
        }
        public ActionResult Search(string query, bool ret = false)
        {
            ViewBag.query = query;
            query = FunctionsMusic.Econde(query);
            ViewBag.search = FunctionsMusic.SearchLastFm(query);
     
            return View();
        }

        public ActionResult SearchArtist(string query)
        {
            ViewBag.data = FunctionsMusic.SearchArtist(query);

            return View();
        }

        public ActionResult GetRelated(string artist, string track, int index = 0)
        {
            artist = Url.Encode(artist.Trim());
            track = Url.Encode(track.Trim());
            var data = FunctionsMusic.GetSimilar(artist, track);
            index += 3;
            dynamic c = JObject.Parse(data);
            var obj = c.similartracks.track[index];
            var output = new { track = obj.name, artist = obj.artist.name, source = "Related Track" };
            var img = obj.image[3].text;
            if (output.track == "")
            {

                index -= 3;
                dynamic top = JObject.Parse(FunctionsMusic.GetTopTracks(artist));
                img = top.tracks.toptracks[index].image[3].text;
                output = new
                {
                    track = top.toptracks.track[index].name,
                    artist = top.toptracks.track[index].artist.name,
                    source = "Top Artist"
                };
                if (top.toptracks.track[index].name != "")
                {
                    artist = Url.Encode(output.artist);
                    track = Url.Encode(output.track);
                    top = JObject.Parse(FunctionsMusic.GetSimilar(artist, track));
                    output = new
                    {
                        track = top.similartracks.track[index].name,
                        artist = top.similartracks.track[index].artist.name,
                        source = "Related Track 2"
                    };
                }


            }

            if (img == "")
                img = "/images/no-cover.png";
            var outputf = new
            {
                artist = output.artist,
                track = output.track,
                source = output.source,
                cover = img,
                key2 = SHA1.Create(output.track + output.artist)
            };
            return Json(outputf);
        }

        public ActionResult GetTopArtist(bool ret = false)
        {
            ViewBag.top = FunctionsMusic.GetTopArtist();
            ViewBag.ret = ret;
            return View();
        }

        public ActionResult GetTopArtistCustom(bool ret)
        {
            //ViewBag.top = FunctionsMusic.get
            return View();
        }

        public ActionResult GetNewReleases(bool ret = false)
        {
            ViewBag.releases = FunctionsMusic.GetNewReleases();
            ViewBag.ret = ret;
            return View();
        }

        public ActionResult GetTopTags(string tag, bool ret = false)
        {
            if ((tag != "") && (tag != "all")) { }
            {
                ViewBag.top = FunctionsMusic.GetTopTags(tag);
                ViewBag.title = Server.UrlDecode(tag);
            }
            ViewBag.ret = ret;
            return View();
        }

        public ActionResult GetArtistInfo(string artist, bool ret = false)
        {
            ViewBag.artist = FunctionsMusic.GetArtistInfo(artist);
            ViewBag.toptracks = FunctionsMusic.GetTopTracks(artist);
            ViewBag.ret = ret;
            return View();
        }

        public ActionResult GetSongInfo(string artist, string track, bool ret = false, string extra = "")
        {
            ViewBag.song = FunctionsMusic.GetTrackInfo(artist, track);
            ViewBag.lyrics = FunctionsMusic.GetLyric(artist, track);
            if (extra != "")
            {
                ViewBag.artist = extra;
            }
            else
            {
                ViewBag.artist = FunctionsMusic.GetArtistInfo(artist);
            }
            ViewBag.toptracks = FunctionsMusic.GetTopTracks(artist);
            ViewBag.ret = ret;
            return View();
        }

        public ActionResult GetAlbums(string artist)
        {
            ViewBag.artist = FunctionsMusic.GetArtistInfo(artist);
            ViewBag.albums = FunctionsMusic.GetAlbums(artist);
            return View();
        }

        public ActionResult GetEvents(string artist)
        {
            ViewBag.artist = artist;
            ViewBag.events = FunctionsMusic.GetEvents(artist);
            return View();
        }

        public ActionResult GetTracksAlbums(string artist, string album)
        {
            ViewBag.events = FunctionsMusic.GetTracksAlbums(album, artist);

            return View();
        }

        public ActionResult GetSearchBox(bool ret = false)
        {
            ViewBag.ret = ret;
            return View();
        }

        public ActionResult GetLyric(string artist, string track)
        {
            ViewBag.lyrics = FunctionsMusic.GetLyric(artist, track);
            ViewBag.title = artist + " - " + track;
            return View();
        }

        public ActionResult GetYoutube(string artist, string track, string picture)
        {
            string[] args = {"http://", "https://", "ftp://", "ftps://", "smtp://", "sftp;//"};
            picture = args.Aggregate(picture, (current, arg) => current.Replace(arg, ""));
            picture = "http://" + picture;
            dynamic data = JObject.Parse(FunctionsMusic.SearchYoutube(track+" "+artist));
            //if(data.data.items[0].id  === undefined)
                 var id = data.items[0].id;
            var tab = new JObject(new JProperty("id",id));
                 return Content(tab.ToString(), "application/json");
        }

        public ActionResult GetTopTracks()
        {
            ViewBag.top = FunctionsMusic.GetTopTracks();
            return View();
        }
    }
}
