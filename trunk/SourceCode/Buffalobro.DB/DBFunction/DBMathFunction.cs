using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.DataBaseAdapter;

namespace Buffalobro.DB.DBFunction
{

    /// <summary>
    /// 数学函数的调用
    /// </summary>
    public class DBMathFunction
    {

        public static string DoAbs(string[] values,DBInfo info) 
        {
            return info.Math.DoAbs(values);
        }
        public static string DoAcos(string[] values,DBInfo info) 
        {
            return info.Math.DoAcos(values);
        }
        public static string DoAsin(string[] values,DBInfo info) 
        {
            return info.Math.DoAsin(values);
        }
        public static string DoAtan(string[] values,DBInfo info) 
        {
            return info.Math.DoAtan(values);
        }
        public static string DoAtan2(string[] values,DBInfo info) 
        {
            return info.Math.DoAtan2(values);
        }
        public static string DoCeil(string[] values,DBInfo info) 
        {
            return info.Math.DoCeil(values);
        }
        public static string DoCos(string[] values,DBInfo info) 
        {
            return info.Math.DoCos(values);
        }
        public static string DoExp(string[] values,DBInfo info) 
        {
            return info.Math.DoExp(values);
        }
        public static string DoFloor(string[] values,DBInfo info) 
        {
            return info.Math.DoFloor(values);
        }
        public static string DoLn(string[] values,DBInfo info)
        {
            return info.Math.DoLn(values);
        }
        public static string DoLog10(string[] values,DBInfo info)
        {
            return info.Math.DoLog10(values);
        }
        public static string DoPower(string[] values,DBInfo info)
        {
            return info.Math.DoPower(values);
        }
        public static string DoRandom(string[] values,DBInfo info)
        {
            return info.Math.DoRandom(values);
        }
        public static string DoRound(string[] values,DBInfo info)
        {
            return info.Math.DoRound(values);
        }
        public static string DoSign(string[] values,DBInfo info)
        {
            return info.Math.DoSign(values);
        }
        public static string DoSin(string[] values,DBInfo info)
        {
            return info.Math.DoSin(values);
        }
        public static string DoSqrt(string[] values,DBInfo info)
        {
            return info.Math.DoSqrt(values);
        }
        public static string DoTan(string[] values,DBInfo info)
        {
            return info.Math.DoTan(values);
        }
        public static string IndexOf(string[] values,DBInfo info)
        {
            return info.Math.IndexOf(values);
        }
        public static string SubString(string[] values,DBInfo info)
        {
            return info.Math.SubString(values);
        }
        
    }
}
