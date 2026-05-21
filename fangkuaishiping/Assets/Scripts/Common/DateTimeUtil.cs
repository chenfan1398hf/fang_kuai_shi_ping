using System;
using UnityEngine;
/// <summary>
/// 时间工具类
/// </summary>
public static class DateTimeUtil
{
    /// <summary>
    /// 时间戳计时开始时间
    /// </summary>
    private static DateTime timeStampStartTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// DateTime转换为10位时间戳（单位：秒）
    /// </summary>
    /// <param name="dateTime"> DateTime</param>
    /// <returns>10位时间戳（单位：秒）</returns>
    public static long DateTimeToTimeStamp(DateTime dateTime)
    {
        return (long)(dateTime.ToUniversalTime() - timeStampStartTime).TotalSeconds;
    }

    /// <summary>
    /// DateTime转换为13位时间戳（单位：毫秒）
    /// </summary>
    /// <param name="dateTime"> DateTime</param>
    /// <returns>13位时间戳（单位：毫秒）</returns>
    public static long DateTimeToLongTimeStamp(DateTime dateTime)
    {
        return (long)(dateTime.ToUniversalTime() - timeStampStartTime).TotalMilliseconds;
    }

    /// <summary>
    /// 10位时间戳（单位：秒）转换为DateTime
    /// </summary>
    /// <param name="timeStamp">10位时间戳（单位：秒）</param>
    /// <returns>DateTime</returns>
    public static DateTime TimeStampToDateTime(long timeStamp)
    {
        return timeStampStartTime.AddSeconds(timeStamp).ToLocalTime();
    }

    /// <summary>
    /// 13位时间戳（单位：毫秒）转换为DateTime
    /// </summary>
    /// <param name="longTimeStamp">13位时间戳（单位：毫秒）</param>
    /// <returns>DateTime</returns>
    public static DateTime LongTimeStampToDateTime(long longTimeStamp)
    {
        return timeStampStartTime.AddMilliseconds(longTimeStamp).ToLocalTime();
    }
    /// <summary>
    /// Sat, 05 Nov 2005 14:06:25 GMT 
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string ToEnglishDateTimeString(DateTime dt)
    {
        if (dt != null)
        {
            string str = dt.GetDateTimeFormats('r')[0].ToString();//Sat, 05 Nov 2005 14:06:25 GMT 
            return str.Substring(4, str.Length - 11);
        }
        else
        {
            return null;
        }
    }
    public static string ToEnglishDateTimeString(long ldt)
    {
        var dt = TimeStampToDateTime(ldt);
        if (dt != null)
        {
            return dt.ToString("d");
        }
        else
        {
            return null;
        }
    }


    /**
     * @Description: 返回日时分秒 格式：days:HI24:mm:ss 小时为24小时制 00:23:15:00
     */
    public static String secondToTime(long seconds)
    {
        long days = seconds / 86400;//转换天数
        seconds = seconds % 86400;//剩余秒数
        long hours = seconds / 3600;//转换小时数
        seconds = seconds % 3600;//剩余秒数
        long minutes = seconds / 60;//转换分钟
        seconds = seconds % 60;//剩余秒数
        string day = string.Format("{0:D2}", days);
        string hour = string.Format("{0:D2}", hours);
        string minute = string.Format("{0:D2}", minutes);
        string second = string.Format("{0:D2}", seconds);
        string ddHHmmss = day + ":" + hour + ":" + minute + ":" + second;
        return ddHHmmss;
    }

    /**
      * @Description: 返回分秒 格式：hh:mm:ss 小时为24小时制 00:15:00
      */
    public static String secondToHHMMSS(long seconds)
    {
        long hours = seconds / 3600;//转换小时数
        seconds = seconds % 3600;//剩余秒数
        long minutes = seconds / 60;//转换分钟
        seconds = seconds % 60;//剩余秒数
 
        string hour = string.Format("{0:D2}", hours);
        string minute = string.Format("{0:D2}", minutes);
        string second = string.Format("{0:D2}", seconds);
        string ddHHmmss =  hour + ":" + minute + ":" + second;
        return ddHHmmss;
    }
    /**
      * @Description: 返回分秒 格式：mm:ss 小时为24小时制 15:00
      */
    public static String secondToMMSS(long seconds)
    {
        long minutes = seconds / 60;//转换分钟
        seconds = seconds % 60;//剩余秒数
        string minute = string.Format("{0:D2}", minutes);
        string second = string.Format("{0:D2}", seconds);
        string ddHHmmss =  minute + ":" + second;
        return ddHHmmss;
    }
}

