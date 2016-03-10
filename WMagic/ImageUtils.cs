using System.IO;
using System.Windows.Media.Imaging;

namespace WMagic
{
    /// <remarks>
    /// -----------------------------------------------------------------------
    /// 部件名：ImageUtils
    /// 工程名：WMagic
    /// 版权：CopyRight (c) 2013
    /// 创建人：WY
    /// 描述：图片辅助类
    /// 创建日期：2013.05.13
    /// 修改人：
    /// 修改日期：
    /// -----------------------------------------------------------------------
    /// </remarks>

    /// <summary>
    /// 图片辅助类
    /// </summary>
    public class ImageUtils
    {
        /// <summary>
        /// 将BitmapImage转换为字节流
        /// </summary>
        /// <param name="ibmp">BitmapImage</param>
        /// <returns>字节流</returns>
        public static byte[] Format(BitmapImage ibmp)
        {
            byte[] data = null;
            {
                if (!MatchUtils.IsEmpty(ibmp))
                {
                    using (Stream stream = ibmp.StreamSource)
                    {
                        if (stream != null && stream.Length > 0)
                        {
                            stream.Position = 0;
                            try
                            {
                                using (BinaryReader reader = new BinaryReader(stream))
                                {
                                    data = reader.ReadBytes((int)stream.Length);
                                }
                            }
                            catch
                            {
                                data = null;
                            }
                        }
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// 将字节流转换为BitmapImage
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns>BitmapImage</returns>
        public static BitmapImage Format(byte[] data)
        {
            if (!MatchUtils.IsEmpty(data))
            {
                BitmapImage ibmp = null;
                try
                {
                    using (MemoryStream stream = new MemoryStream(data))
                    {
                        (ibmp = new BitmapImage()).BeginInit();
                        {
                            ibmp.CacheOption = BitmapCacheOption.OnLoad;
                            {
                                ibmp.StreamSource = stream;
                            }
                        }
                        ibmp.EndInit();
                    }
                }
                catch
                {
                    ibmp = null;
                }
                finally
                {
                    if (ibmp != null && ibmp.CanFreeze)
                    {
                        ibmp.Freeze();
                    }
                }
                return ibmp;
            }
            return null;
        }

    }
}