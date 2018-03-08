using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPX
{
    public class Navigation
    {
        private List<trkpt> track = null;
        private int maxHDOPValue;

        public Navigation(List<trkpt> track, int maxHDOPValue)
        {
            this.track = track;
            this.maxHDOPValue = maxHDOPValue;
        }

        private void cleanBadData()
        {
            for (int i = 0; i < track.Count; i++)
            {
                if (track[i].hdop > maxHDOPValue)
                {
                    track.Remove(track[i]);
                }
            }
        }

        private double getDist(trkpt a, trkpt b)
        {
            var phi1 = a.lat * Constants.PI / 180;
            var phi2 = b.lat * Constants.PI / 180;
            var deltaPhi = (b.lat - a.lat) * Constants.PI / 180;
            var deltalambda = (b.lon - a.lon) * Constants.PI / 180;
            var x = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                    Math.Cos(phi1) * Math.Cos(phi2) * Math.Sin(deltalambda / 2) * Math.Sin(deltalambda / 2);
            var y = 2 * Math.Atan2(Math.Sqrt(x), Math.Sqrt(1 - x));
            return Constants.Radius * y;
        }

        public double distanceInMeters()
        {
            double res = 0;
            for (int i = 0; i < track.Count - 1; i++)
            {
                res += getDist(track[i], track[i + 1]);
            }
            return res;
        }

        public double avgSpeedKMH()
        {
            double avgSpeed = 0;
            int i = 0;
            for (i = 0; i < track.Count - 1; i++)
            {
                var deltaTime = (track[i + 1].dateTime - track[i].dateTime).TotalSeconds;
                var deltaDist = getDist(track[i], track[i + 1]);
                avgSpeed += deltaDist / deltaTime;
            }
            avgSpeed /= i;
            return avgSpeed * 3.6;
        }

        public double maxSpeedKMH()
        {
            double maxSpeed = 0;
            for (int i = 0; i < track.Count - 1; i++)
            {
                var deltaTime = (track[i + 1].dateTime - track[i].dateTime).TotalSeconds;
                var deltaDist = getDist(track[i], track[i + 1]);
                if (deltaDist / deltaTime > maxSpeed)
                {
                    maxSpeed = deltaDist / deltaTime;
                }
            }
            return maxSpeed * 3.6;
        }

        public double minSpeedKMH()
        {
            double minSpeed = 1e6;
            for (int i = 0; i < track.Count - 1; i++)
            {
                var deltaTime = (track[i + 1].dateTime - track[i].dateTime).TotalSeconds;
                var deltaDist = getDist(track[i], track[i + 1]);
                if (deltaDist / deltaTime < minSpeed)
                {
                    minSpeed = deltaDist / deltaTime;
                }
            }
            return minSpeed * 3.6;
        }

        public double durationInMinutes()
        {
            double duration = 0;
            for (int i = 0; i < track.Count - 1; i++)
            {
                var deltaTime = (track[i + 1].dateTime - track[i].dateTime).TotalSeconds;
                duration += deltaTime;
            }
            return duration / 60;
        }
    }
}
