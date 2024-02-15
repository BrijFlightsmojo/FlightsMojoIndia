using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MojoIndia.Controllers
{
    public static class StringHelper
    {
        private static Random random = new Random();
        public static string CompressString(this String _content)
        {
            if (string.IsNullOrEmpty(_content))
                return string.Empty;

            MemoryStream memoryStream = null, outStream = null;
            GZipStream zip = null;

            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(_content);
                memoryStream = new MemoryStream();

                using (zip = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    zip.Write(buffer, 0, buffer.Length);
                }

                memoryStream.Position = 0;
                outStream = new MemoryStream();

                byte[] compressed = new byte[memoryStream.Length];
                memoryStream.Read(compressed, 0, compressed.Length);

                byte[] gzBuffer = new byte[compressed.Length + 4];
                Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
                Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
                return Convert.ToBase64String(gzBuffer);

            }
            finally
            {
                #region Release objects

                if (zip != null)
                {
                    zip.Dispose();
                    zip = null;
                }

                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                    memoryStream = null;
                }

                if (outStream != null)
                {
                    outStream.Dispose();
                    outStream = null;
                }

                #endregion
            }
        }
        public static string DecompressString(this String _content)
        {
            if (string.IsNullOrEmpty(_content))
                return string.Empty;

            GZipStream zip = null;
            MemoryStream memoryStream = null;

            try
            {
                byte[] gzBuffer = Convert.FromBase64String(_content);
                memoryStream = new MemoryStream();

                int contentLength = BitConverter.ToInt32(gzBuffer, 0);
                memoryStream.Write(gzBuffer, 4, gzBuffer.Length - 4);

                byte[] buffer = new byte[contentLength];

                memoryStream.Position = 0;
                zip = new GZipStream(memoryStream, CompressionMode.Decompress);

                zip.Read(buffer, 0, buffer.Length);

                return Encoding.UTF8.GetString(buffer);
            }
            /* Catch the format exception, Some of the old data may be in
             * decompressed mode So in this case only return the content 
             * passed as paramert.
             */
            catch (FormatException)
            {
                return _content;
               
            }
            finally
            {
                #region Release Objects

                if (zip != null)
                {
                    zip.Dispose();
                    zip = null;
                }

                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                    memoryStream = null;
                }

                #endregion
            }
        }

        public static string SerializeObject(object obj)
        {

            System.Xml.XmlDocument doc = new XmlDocument();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            string serializedString = string.Empty;
            try
            {
                if (obj != null)
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                    serializer.Serialize(stream, obj);
                    stream.Position = 0;
                    doc.Load(stream);
                    serializedString = doc.InnerXml;
                }
            }
            catch (Exception ex)           {
                

            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }

            return serializedString;
        }
        public static object DeSerializeObject(string xmlOfAnObject)
        {


            object myObject = new object();
            System.IO.StringReader read = new StringReader(xmlOfAnObject);
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(myObject.GetType());
            System.Xml.XmlReader reader = new XmlTextReader(read);
            try
            {
                myObject = (object)serializer.Deserialize(reader);
                return myObject;
            }
            catch
            {
                throw;
            }
            finally
            {
                reader.Close();
                read.Close();
                read.Dispose();
            }
        }

        public static T Deserialize<T>(string input) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        public static string Serialize<T>(T ObjectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ObjectToSerialize);
                return textWriter.ToString();
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
