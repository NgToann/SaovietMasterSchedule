using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MasterSchedule.Helpers
{
    class TimeHelper
    {
        /// <summary>
        /// 0 return format: dd-MMM  1 return format: M/d
        /// </summary>
        /// <param name="date"></param>
        /// <param name="type">0:dd-MMM 1:M/d</param>
        /// <returns></returns>
        public static string DisplayDate(DateTime date, int type)
        {
            string result = "";
            if (type == 0)
                result = String.Format(new CultureInfo("en-US"), "{0:dd-MMM}", date);
            if (type == 1)
                result = String.Format("{0:M/d}", date);
            return result;
        }
        public static DateTime Convert(string input)
        {
            DateTime output = new DateTime(2000, 1, 1);
            if (String.IsNullOrEmpty(input))
                return output;
            try
            {
                if (input.Contains("-"))
                    input.Replace('-', '/');
                string[] inputSplit = input.Split('/');
                int day = int.Parse(inputSplit[1]);
                int month = int.Parse(inputSplit[0]);
                int year = DateTime.Now.Year;
                if (inputSplit.Count() == 3)
                {
                    year = int.Parse(inputSplit[2]);
                }
                output = new DateTime(year, month, day);
            }
            catch
            {
                if (String.IsNullOrEmpty(input) == false)
                {
                    //output = new DateTime(2000, 1, 1);
                    output = new DateTime(1999, 12, 31);
                }
            }
            return output;
        }

        public static string ConvertDateToView(string input)
        {
            string result = "";
            if (String.IsNullOrEmpty(input) == true)
            {
                return result;
            }
            else
            {
                return result = String.Format("{0:M/d}", TimeHelper.Convert(input));
            }
        }
    }
}
