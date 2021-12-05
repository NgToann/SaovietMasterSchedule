using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MasterSchedule.Models;

namespace MasterSchedule.Helpers
{
    public class SizeHelper
    {
        /// <summary>
        /// Sort SizeNoList
        /// </summary>
        /// <param name="sizeNoRequest"></param>
        /// <returns></returns>
        public static void Sort(List<string> sizeNoRequest)
        {
            var regex = new Regex("[a-z]|[A-Z]");
            if (sizeNoRequest.Count() > 0)
                sizeNoRequest = sizeNoRequest.OrderBy(s => regex.IsMatch(s) ? Double.Parse(regex.Replace(s, "100")) : Double.Parse(s)).ToList();
        }

        /// <summary>
        /// Sort SizeRunList
        /// </summary>
        /// <param name="sizeRunList"></param>
        /// <returns></returns>
        public static List<string> Sort(List<SizeRunModel> sizeRunList)
        {
            var regex = new Regex("[a-z]|[A-Z]");
            var sizeNoList = sizeRunList.Select(s => s.SizeNo).Distinct().ToList();
            if (sizeNoList.Count() > 0)
                return sizeNoList.OrderBy(s => regex.IsMatch(s) ? Double.Parse(regex.Replace(s, "100")) : Double.Parse(s)).ToList();
            return new List<string>();
        }
    }
}
