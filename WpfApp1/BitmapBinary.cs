using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WpfApp1
{
    class BitmapBinary
    {
        //typedef struct tagBITMAPFILEHEADER {全14Byte  
        //        WORD    bfType;       2Byte  
        //        DWORD   bfSize;       4Byte  
        //        WORD    bfReserved1;      2Byte  
        //        WORD    bfReserved2;      2Byte  
        //        DWORD   bfOffBits;        4Byte  
        //} BITMAPFILEHEADER;  
        public struct BITMAPFILEHEADER
        {
            /// <summary>  
            /// ファイルタイプ  
            /// </summary>  
            public UInt16 bfType;
            /// <summary>  
            /// ファイル全体のサイズ  
            /// </summary>  
            public UInt32 bfSize;
            /// <summary>  
            /// 予約領域  
            /// </summary>  
            public UInt16 bfReserved1;
            /// <summary>  
            /// 予約領域  
            /// </summary>  
            public UInt16 bfReserved2;
            /// <summary>  
            /// ファイルの先頭から画像データまでのオフセット数（バイト数）  
            /// </summary>  
            public UInt32 bfOffBits;
        }

        //typedef struct tagBITMAPINFOHEADER{全40Byte  
        //        DWORD      biSize;        4Byte  
        //        LONG       biWidth;       4Byte  
        //        LONG       biHeight;      4Byte  
        //        WORD       biPlanes;      2Byte  
        //        WORD       biBitCount;    2Byte  
        //        DWORD      biCompression; 4Byte  
        //        DWORD      biSizeImage;   4Byte  
        //        LONG       biXPelsPerMeter;   4Byte  
        //        LONG       biYPelsPerMeter;   4Byte  
        //        DWORD      biClrUsed;     4Byte  
        //        DWORD      biClrImportant;    4Byte  
        //} BITMAPINFOHEADER;  
        public struct BITMAPINFOHEADER
        {
            /// <summary>  
            /// BITMAPINFOHEADERのサイズ (40)  
            /// </summary>  
            public UInt32 biSize;
            /// <summary>  
            /// ビットマップの幅  
            /// </summary>  
            public Int32 biWidth;
            /// <summary>  
            /// ビットマップの高さ  
            /// </summary>  
            public Int32 biHeight;
            /// <summary>  
            /// プレーン数(常に1)  
            /// </summary>  
            public UInt16 biPlanes;
            /// <summary>  
            /// 1ピクセルあたりのビット数(1,4,8,16,24,32)  
            /// </summary>  
            public UInt16 biBitCount;
            /// <summary>  
            /// 圧縮形式  
            /// </summary>  
            public UInt32 biCompression;
            /// <summary>  
            /// イメージのサイズ(バイト数)  
            /// </summary>  
            public UInt32 biSizeImage;
            /// <summary>  
            /// ビットマップの水平解像度  
            /// </summary>  
            public Int32 biXPelsPerMeter;
            /// <summary>  
            /// ビットマップの垂直解像度  
            /// </summary>  
            public Int32 biYPelsPerMeter;
            /// <summary>  
            /// カラーパレット数  
            /// </summary>  
            public UInt32 biClrUsed;
            /// <summary>  
            /// 重要なカラーパレットのインデックス  
            /// </summary>  
            public UInt32 biClrImportant;
        }

        /// <summary>  
        /// ビットマップファイルをバイナリで開く  
        /// </summary>  
        /// <param name="FileName">ビットマップファイル名(*.bmp)</param>  
        /// <param name="bfh">BITMAPFILEHEADER</param>  
        /// <param name="bih">BITMAPINFOHEADER</param>  
        /// <param name="ColorPal">カラーパレット</param>  
        /// <param name="BitData">画像のデータ（輝度値）</param>  
        public static bool Load(
            String FileName,
            out BITMAPFILEHEADER bfh,
            out BITMAPINFOHEADER bih,
            out System.Drawing.Color[] ColorPal,
            out byte[] BitData)
        {

            int i;

            // 拡張子の確認  
            String ext = Path.GetExtension(FileName).ToLower();
            if (ext != ".bmp") goto ErrorHandler;

            // データ読込用配列の確保  
            byte[] ReadData = new byte[4];

            // ファイルを開く  
            FileStream fs;
            try
            {
                fs = File.Open(FileName, FileMode.Open, FileAccess.Read);
                if (fs == null) goto ErrorHandler;
            }
            catch
            {
                goto ErrorHandler;
            }

            //////////////////////////////////  
            //  
            // BITMAPFILEHEADERの読込  
            //  
            //////////////////////////////////  

            // bfType  
            fs.Read(ReadData, 0, 2);
            bfh.bfType = BitConverter.ToUInt16(ReadData, 0);
            // bfSize  
            fs.Read(ReadData, 0, 4);
            bfh.bfSize = BitConverter.ToUInt32(ReadData, 0);
            // bfReserved1  
            fs.Read(ReadData, 0, 2);
            bfh.bfReserved1 = BitConverter.ToUInt16(ReadData, 0);
            // bfReserved2  
            fs.Read(ReadData, 0, 2);
            bfh.bfReserved2 = BitConverter.ToUInt16(ReadData, 0);
            // bfOffBits  
            fs.Read(ReadData, 0, 4);
            bfh.bfOffBits = BitConverter.ToUInt32(ReadData, 0);

            //////////////////////////////////  
            //  
            // BITMAPINFOHEADERの読込  
            //  
            //////////////////////////////////  

            // biSize  
            fs.Read(ReadData, 0, 4);
            bih.biSize = BitConverter.ToUInt32(ReadData, 0);
            // biWidth  
            fs.Read(ReadData, 0, 4);
            bih.biWidth = BitConverter.ToInt32(ReadData, 0);
            // biHeight  
            fs.Read(ReadData, 0, 4);
            bih.biHeight = BitConverter.ToInt32(ReadData, 0);
            // biPlanes  
            fs.Read(ReadData, 0, 2);
            bih.biPlanes = BitConverter.ToUInt16(ReadData, 0);
            //biBitCount  
            fs.Read(ReadData, 0, 2);
            bih.biBitCount = BitConverter.ToUInt16(ReadData, 0);
            // biCompression  
            fs.Read(ReadData, 0, 4);
            bih.biCompression = BitConverter.ToUInt32(ReadData, 0);
            // biSizeImage  
            fs.Read(ReadData, 0, 4);
            bih.biSizeImage = BitConverter.ToUInt32(ReadData, 0);
            // biXPelsPerMeter  
            fs.Read(ReadData, 0, 4);
            bih.biXPelsPerMeter = BitConverter.ToInt32(ReadData, 0);
            // biYPelsPerMeter  
            fs.Read(ReadData, 0, 4);
            bih.biYPelsPerMeter = BitConverter.ToInt32(ReadData, 0);
            // biClrUsed  
            fs.Read(ReadData, 0, 4);
            bih.biClrUsed = BitConverter.ToUInt32(ReadData, 0);
            // biClrImportant  
            fs.Read(ReadData, 0, 4);
            bih.biClrImportant = BitConverter.ToUInt32(ReadData, 0);

            //////////////////////////////////  
            //  
            // カラーパレットの読込  
            //  
            //////////////////////////////////  

            // カラーパレットのサイズの計算  
            //   bfOffBitsからBITMAPFILEHEADERとBITMAPINFOHEADERのサイズ文を  
            //   引いたのがカラーパレットのサイズ  
            long PalSize = (bfh.bfOffBits - 14 - 40) / 4;

            if (PalSize != 0)
            {
                ColorPal = new System.Drawing.Color[PalSize];

                for (i = 0; i < PalSize; i++)
                {
                    fs.Read(ReadData, 0, 4);
                    ColorPal[i] =
                        System.Drawing.Color.FromArgb(
                                ReadData[3],
                                ReadData[2],
                                ReadData[1],
                                ReadData[0]);
                }
            }
            else
            {
                ColorPal = null;
            }

            //////////////////////////////////  
            //  
            // 画像データ（輝度値）の読込  
            //  
            //////////////////////////////////  

            // 画像データの幅（バイト数）の計算  
            int Stride = ((bih.biWidth * bih.biBitCount + 31) / 32) * 4;

            //メモリの確保  
            BitData = new Byte[Stride * bih.biHeight];

            //画像データを画像の下側から読み込む（上下を反転させて読み込む）  
            for (int j = bih.biHeight - 1; j >= 0; j--)
            {
                fs.Read(BitData, j * Stride, Stride);
            }

            //ファイルを閉じる  
            fs.Close();

            // 解放  
            fs.Dispose();

            // 正常読込  
            return true;

        //////////////////////////////////  
        //  
        // エラー処理  
        //  
        //////////////////////////////////  
        ErrorHandler:
            bfh.bfType = 0;
            bfh.bfSize = 0;
            bfh.bfReserved1 = 0;
            bfh.bfReserved2 = 0;
            bfh.bfOffBits = 0;

            bih.biSize = 0;
            bih.biWidth = 0;
            bih.biHeight = 0;
            bih.biPlanes = 0;
            bih.biBitCount = 0;
            bih.biCompression = 0;
            bih.biSizeImage = 0;
            bih.biXPelsPerMeter = 0;
            bih.biYPelsPerMeter = 0;
            bih.biClrUsed = 0;
            bih.biClrImportant = 0;

            ColorPal = null;
            BitData = null;

            //読込失敗  
            return false;
        }

        /// <summary>  
        /// バイナリデータをビットマップファイルに保存する  
        /// </summary>  
        /// <param name="FileName">ビットマップファイル名(*.bmp)</param>  
        /// <param name="Width">ビットマップの幅</param>  
        /// <param name="Height">ビットマップの高さ</param>  
        /// <param name="BitCount">ビット数</param>  
        /// <param name="BitData">画像のデータ（輝度値）</param>  
        public static bool Save(
            String FileName,
            int Width,
            int Height,
            int BitCount,
            byte[] BitData)
        {
            //配列の有無の確認  
            if (BitData == null) return false;

            // 拡張子の確認  
            String ext = Path.GetExtension(FileName).ToLower();
            if (ext != ".bmp") return false; ;

            // 画像データの幅（バイト数）の計算  
            int Stride = ((Width * BitCount + 31) / 32) * 4;

            //画像データサイズの確認  
            if (BitData.Length != Stride * Height) return false;

            //カラーパレットの個数  
            UInt32 PalSize;
            byte[] ColorPal = null;
            if (BitCount == 8)
            {
                PalSize = 256;
                //カラーパレットをバイト配列で確保  
                ColorPal = new byte[PalSize * 4];
                for (int i = 0; i < 256; i++)
                {
                    ColorPal[i * 4] = (byte)i;    //B  
                    ColorPal[i * 4 + 1] = (byte)i;    //G  
                    ColorPal[i * 4 + 2] = (byte)i;    //R  
                    ColorPal[i * 4 + 3] = 0;          //A  
                }
            }
            else
            {
                PalSize = 0;
            }

            //BITMAPFILEHEADERの作成  
            BITMAPFILEHEADER bfh;
            bfh.bfType = 0x4d42;
            //bfh.bfSize = 0;  
            bfh.bfReserved1 = 0;
            bfh.bfReserved2 = 0;
            bfh.bfOffBits = 14 + 40 + PalSize * 4;
            bfh.bfSize = bfh.bfOffBits + (uint)(Stride * Height);

            //BITMAPINFOHEADERの作成  
            BITMAPINFOHEADER bih;
            bih.biSize = 40;
            bih.biWidth = Width;
            bih.biHeight = Height;
            bih.biPlanes = 1;
            bih.biBitCount = (ushort)BitCount;
            bih.biCompression = 0;
            bih.biSizeImage = 0;
            bih.biXPelsPerMeter = 0;
            bih.biYPelsPerMeter = 0;
            bih.biClrUsed = PalSize;
            bih.biClrImportant = PalSize;

            // ファイルを開く  
            FileStream fs;
            try
            {
                fs = File.Open(FileName, FileMode.Create, FileAccess.Write);
                if (fs == null) return false;
            }
            catch
            {
                return false;
            }

            //////////////////////////////////  
            //  
            // BITMAPFILEHEADERの書込  
            //  
            //////////////////////////////////  

            // bfType  
            fs.Write(BitConverter.GetBytes(bfh.bfType), 0, 2);
            // bfSize  
            fs.Write(BitConverter.GetBytes(bfh.bfSize), 0, 4);
            // bfReserved1  
            fs.Write(BitConverter.GetBytes(bfh.bfReserved1), 0, 2);
            // bfReserved2  
            fs.Write(BitConverter.GetBytes(bfh.bfReserved2), 0, 2);
            // bfOffBits  
            fs.Write(BitConverter.GetBytes(bfh.bfOffBits), 0, 4);

            //////////////////////////////////  
            //  
            // BITMAPINFOHEADERの書込  
            //  
            //////////////////////////////////  

            // biSize  
            fs.Write(BitConverter.GetBytes(bih.biSize), 0, 4);
            // biWidth  
            fs.Write(BitConverter.GetBytes(bih.biWidth), 0, 4);
            // biHeight  
            fs.Write(BitConverter.GetBytes(bih.biHeight), 0, 4);
            // biPlanes  
            fs.Write(BitConverter.GetBytes(bih.biPlanes), 0, 2);
            //biBitCount  
            fs.Write(BitConverter.GetBytes(bih.biBitCount), 0, 2);
            // biCompression  
            fs.Write(BitConverter.GetBytes(bih.biCompression), 0, 4);
            // biSizeImage  
            fs.Write(BitConverter.GetBytes(bih.biSizeImage), 0, 4);
            // biXPelsPerMeter  
            fs.Write(BitConverter.GetBytes(bih.biXPelsPerMeter), 0, 4);
            // biYPelsPerMeter  
            fs.Write(BitConverter.GetBytes(bih.biYPelsPerMeter), 0, 4);
            // biClrUsed  
            fs.Write(BitConverter.GetBytes(bih.biClrUsed), 0, 4);
            // biClrImportant  
            fs.Write(BitConverter.GetBytes(bih.biClrImportant), 0, 4);

            //////////////////////////////////  
            //  
            // カラーパレットの書込  
            //  
            //////////////////////////////////  

            if (PalSize != 0)
            {
                fs.Write(ColorPal, 0, ColorPal.Length);
            }

            //////////////////////////////////  
            //  
            // 画像データ（輝度値）の書込  
            //  
            //////////////////////////////////  

            //画像データを画像の下側から書き込む（上下を反転させて読み込む）  
            for (int j = bih.biHeight - 1; j >= 0; j--)
            {
                fs.Write(BitData, j * Stride, Stride);
            }

            //ファイルを閉じる  
            fs.Close();

            // 解放  
            fs.Dispose();

            // 正常書込  
            return true;

        }
    }
}
