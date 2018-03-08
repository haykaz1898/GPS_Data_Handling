using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace GPX
{
    public static class GPXReader
    {
        public static List<trkpt> Reader(string fileName)
        {
            List<trkpt> track = new List<trkpt>();
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            #region parsing
            foreach (XmlNode trk in doc.DocumentElement)
            {
                foreach (XmlNode trkseg in trk)
                {
                    foreach (XmlNode trkpt in trkseg)
                    {
                        trkpt tmp = new trkpt();
                        double.TryParse(trkpt.Attributes[0].Value, out tmp.lat);
                        double.TryParse(trkpt.Attributes[1].Value, out tmp.lon);
                        float.TryParse(trkpt["ele"].InnerText, out tmp.ele);
                        float.TryParse(trkpt["hdop"].InnerText, out tmp.hdop);
                        string dateTime = trkpt["time"].InnerText;

                        dateTime = dateTime.Replace("T", " ");
                        dateTime = dateTime.Replace("Z", " ");

                        DateTime dt = Convert.ToDateTime(dateTime);
                        tmp.dateTime = dt;
                        track.Add(tmp);
                    }
                }

            }
            #endregion
            return track;
        }
    }
}
