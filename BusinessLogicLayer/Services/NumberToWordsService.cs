using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Services
{
    /// <summary>
    /// Service for NumberToWordsService, include all methods that help you to convert number to word.
    /// </summary>
    public static class NumberToWordsService
    {
        private static string[] unitsEng = { "Zero", "One", "Two", "Three",
                                           "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
                                           "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
                                           "Seventeen", "Eighteen", "Nineteen" };
        private static string[] tensEng = { "", "", "Twenty", "Thirty", "Forty",
                                         "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        private static string[] unitsUkr = { "Нуль", "Одина", "Дві", "Три", "Чотири", "П'ять", "Шість", "Сім", "Вісім", "Дев'ять",
                                            "Десять","Одинадцять","Дванадцять","Тринадцять","Чотирнадцять", "П'ятнадцять",
                                            "Шістнадцять","Сімнадцять", "Вісімнадцять","Дев'ятнадцять"};
        private static string[] tensUkr =   { "", "", "Двадцять", "Тридцять", "Сорок", "П'ятдесят", "Шістдесят", "Сімдесят", "Вісімдесят", "Дев'яносто" };


        /// <summary>
        /// Transform <param name="amount"/> to words, that describes numbers
        /// </summary>
        /// <returns>Converted data</returns>
        public static string ConvertAmountToEng(double amount)
        {
            try
            {
                int amount_int = (int)amount;
                int amount_dec = (int)Math.Round((amount - (double)(amount_int)) * 100);
                if (amount_dec == 0)
                {
                    return ConvertENG(amount_int) + ".";
                }
                else
                {
                    return $"{ConvertENG(amount_int)} Point {ConvertENG(amount_dec)} ";
                }
            }
            catch (Exception e)
            {
                // TODO: handle exception  
            }
            return "";
        }
        /// <summary>
        /// Transform <param name="amount"/> to words, that describes numbers
        /// </summary>
        /// <returns>Converted data</returns>
        public static string ConvertAmountToUkr(double amount)
        {
            try
            {
                int amount_int = (int)amount;
                int amount_dec = (int)Math.Round((amount - (double)(amount_int)) * 100);
                if (amount_dec == 0)
                {
                    return ConvertUKR(amount_int) + ".";
                }
                else
                {
                    return $"{ConvertUKR(amount_int)}.{ConvertUKR(amount_dec)}";
                }
            }
            catch (Exception e)
            {
                // TODO: handle exception  
            }
            return "";
        }
        /// <summary>
        /// Transform <param name="amount"/> to words, that describes numbers
        /// </summary>
        ///  <example> 
        /// This sample shows how to call the <see cref="ConvertENG"/>
        /// 
        /// <code>
        ///  if (i < 100) //check condition, if true 
        //// {
        ///if i = 88             88/10 = 8.8    88 % 10 = 8 > 0    true             88 % 10 = 8 search 8-th element of array and get eighty eight  
        ///   return $" {tensEng[i / 10] +     ((i % 10 >  0 )    ? " " + ConvertENG(i % 10)    : "")} ";
        ///}
        ///
    /// 
    /// </code>
    /// </example>
    /// <returns>Converted data</returns>
    public static string ConvertENG(int i)
        {
            if (i < 20)
            {
                return unitsEng[i];
            }
            if (i < 100)
            {
                return $" {tensEng[i / 10] + ((i % 10 > 0) ? " "+ ConvertENG(i % 10) : "")} ";
            }
            if (i < 1000)
            {
                return $"{unitsEng[i / 100]} Hundred  { ((i % 100 > 0) ? "And " + ConvertENG(i % 100) : "")}";
            }
            if (i < 100000)
            {
                return $"{ ConvertENG(i / 1000)} Thousand { ((i % 1000 > 0) ? " " + ConvertENG(i % 1000) : "")}";
            }
            if (i < 10000000)
            {
                return $"{ConvertENG(i / 100000) } Thousands  {((i % 100000 > 0) ? " " + ConvertENG(i % 100000) : "")}";
            }
            if (i < 1000000000)
            {
                return $"{ConvertENG(i / 10000000)} Million {((i % 10000000 > 0) ? " " + ConvertENG(i % 10000000) : "")}";
            }
            return $"{ConvertENG(i / 1000000000)} Billion {((i % 1000000000 > 0) ? " " + ConvertENG(i % 1000000000) : "")}";
        }

        /// <summary>
        /// Transform <param name="i"/> to words, that describes numbers
        /// </summary>
        ///  <example> 
        /// This sample shows how to call the <see cref="ConvertUKR"/>
        /// 
        /// <code>
        ///  if (i < 100) //check condition, if true 
        //// {
        ///if i = 88             88/10 = 8.8    88 % 10 = 8 > 0    true             88 % 10 = 8 search 8-th element of array and get eighty eight  
        ///   return $" {tensEng[i / 10] +     ((i % 10 >  0 )    ? " " + ConvertUKR(i % 10)    : "")} ";
        ///}
        ///
        /// 
        /// </code>
        /// </example>
        /// <returns>Converted data</returns>
        public static string ConvertUKR(int i)
        {
            if (i < 20)
            {
                return unitsUkr[i];
            }
            if (i < 100)
            {
                return tensUkr[i / 10] + ((i % 10 > 0) ? " " + ConvertUKR(i % 10) : "");
            }
            if (i < 1000)
            {
                return $"{unitsUkr[i / 100]} Сотня {((i % 100 > 0) ? ConvertUKR(i % 100):"") }";
            }
            if (i < 100000)
            {
                return $"{ConvertUKR(i / 1000)} Тисяча { ((i % 1000 > 0) ? " " + ConvertUKR(i % 1000) : "")}";
            }
            if (i < 10000000)
            {
                return $"{ConvertUKR(i / 100000)} Тисяч {((i % 100000 > 0) ? " " + ConvertUKR(i % 100000) : "")}";
            }
            if (i < 1000000000)
            {
                return $"{ConvertUKR(i / 10000000)} Мільйон {((i % 10000000 > 0) ? " " + ConvertUKR(i % 10000000) : "")}";
            }
            return $"{ConvertUKR(i / 1000000000)} Мільярд {((i % 1000000000 > 0) ? " " + ConvertUKR(i % 1000000000) : "")}";
        }
    }
}
