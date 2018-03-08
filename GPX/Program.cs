using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
namespace GPX
{
    class Program
    {
        static void Main(string[] args)
        {
            List<trkpt> track = GPXReader.Reader("SampleTrack.gpx");
            Navigation nav = new Navigation(track,8);
            nav.avgSpeedKMH();
            double min = nav.minSpeedKMH();
            double max = nav.maxSpeedKMH();
            double avg = nav.avgSpeedKMH();
            double dist = nav.distanceInMeters();
            double dur = nav.durationInMinutes();
        }
    }
}
