using BreadCrumbs.Shared.Models;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BreadCrumbs.Shared.Helpers
{
    //public class BlobSerializer : IBlobSerializer
    //{
    //    public bool CanDeserialize(Type type)
    //    {
    //        return (typeof(Place)) == type;
    //    }

    //    public object Deserialize(byte[] arrayData, Type type)
    //    {
    //        ComplexType c;

    //        using (MemoryStream stream = new MemoryStream())
    //        {
    //            stream.Write(arrayData, 0, arrayData.Length);
    //            stream.Seek(0, SeekOrigin.Begin);
    //            BinaryFormatter formatter = new BinaryFormatter();
    //            c = (ComplexType)formatter.Deserialize(stream);
    //        }

    //        return c;
    //    }

    //    public byte[] Serialize<T>(T c)
    //    {
    //        byte[] arrayData;

    //        using (MemoryStream stream = new MemoryStream())
    //        {
    //            BinaryFormatter formatter = new BinaryFormatter();
    //            formatter.Serialize(stream, c);
    //            arrayData = stream.ToArray();
    //            stream.Close();
    //        }

    //        return arrayData;
    //    }
    //}
}
