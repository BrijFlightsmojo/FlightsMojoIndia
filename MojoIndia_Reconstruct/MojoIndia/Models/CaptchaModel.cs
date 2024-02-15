using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;

namespace MojoIndia.Models
{
    public class CaptchaModel
    {
        [Required(ErrorMessage = "Please enter name!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter email id!")]
        [EmailAddress(ErrorMessage = "You have entered an invalid email address!")]
        public string EmailID { get; set; }
        [Required(ErrorMessage = "Please enter correct contact no!")]
        public string PhoneNo { get; set; }
        [Required(ErrorMessage = "Please enter your query!")]
        public string Massege { get; set; }
        //public string CapImage { get; set; }
        //[Required(ErrorMessage = "Verification code is required!")]
        //public string CaptchaCodeText { get; set; }
     //   public string CapImageText { get; set; }
        public string sendMsg { get; set; }
    }
    public class CaptchaUtilityClass
    {
        public byte[] GetCaptchaImage(string checkCode)
        {

            Bitmap image = new Bitmap(Convert.ToInt32(Math.Ceiling((decimal)(checkCode.Length * 15))), 25);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //Random random = new Random();
                g.Clear(Color.AliceBlue);
                Font font = new Font("Comic Sans MS", 16, FontStyle.Bold);
                string str = "";
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                for (int i = 0; i < checkCode.Length; i++)
                {
                    str = str + checkCode.Substring(i, 1);
                }
                g.DrawString(str, font, new SolidBrush(Color.Blue), 0, 0);
                g.Flush();
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
        public byte[] GetCaptchaImageNew(string checkCode)
        {
            Bitmap objBitmap = new Bitmap(130, 60);
            Graphics objGraphics = Graphics.FromImage(objBitmap);
            objGraphics.Clear(Color.White);
            Random objRandom = new Random();
            objGraphics.DrawLine(Pens.Black, objRandom.Next(0, 50), objRandom.Next(10, 30), objRandom.Next(0, 200), objRandom.Next(0, 50));
            objGraphics.DrawRectangle(Pens.Blue, objRandom.Next(0, 20), objRandom.Next(0, 20), objRandom.Next(50, 80), objRandom.Next(0, 20));
            objGraphics.DrawLine(Pens.Blue, objRandom.Next(0, 20), objRandom.Next(10, 50), objRandom.Next(100, 200), objRandom.Next(0, 80));
            Brush objBrush = default(Brush);
            //create background style  
            HatchStyle[] aHatchStyles = new HatchStyle[]
            {
                HatchStyle.BackwardDiagonal, HatchStyle.Cross, HatchStyle.DashedDownwardDiagonal, HatchStyle.DashedHorizontal, HatchStyle.DashedUpwardDiagonal, HatchStyle.DashedVertical,
                    HatchStyle.DiagonalBrick, HatchStyle.DiagonalCross, HatchStyle.Divot, HatchStyle.DottedDiamond, HatchStyle.DottedGrid, HatchStyle.ForwardDiagonal, HatchStyle.Horizontal,
                    HatchStyle.HorizontalBrick, HatchStyle.LargeCheckerBoard, HatchStyle.LargeConfetti, HatchStyle.LargeGrid, HatchStyle.LightDownwardDiagonal, HatchStyle.LightHorizontal
            };
            //create rectangular area  
            RectangleF oRectangleF = new RectangleF(0, 0, 300, 300);
            objBrush = new HatchBrush(aHatchStyles[objRandom.Next(aHatchStyles.Length - 3)], Color.FromArgb((objRandom.Next(100, 255)), (objRandom.Next(100, 255)), (objRandom.Next(100, 255))), Color.White);
            objGraphics.FillRectangle(objBrush, oRectangleF);
            try
            {


                string str = "";
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, objBitmap.Width, objBitmap.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                for (int i = 0; i < checkCode.Length; i++)
                {
                    str = str + checkCode.Substring(i, 1);
                }
                //Generate the image for captcha  
                //string captchaText = string.Format("{0:X}", objRandom.Next(1000000, 9999999));
                //add the captcha value in session  

                Font objFont = new Font("Courier New", 18, FontStyle.Bold);
                //Draw the image for captcha  
                objGraphics.DrawString(str, objFont, Brushes.Black, 20, 20);

                objGraphics.Flush();
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                objBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
            finally
            {

                objGraphics.Dispose();
                objBitmap.Dispose();
            }
        }
        public byte[] VerificationTextGenerator()
        {

            string allChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < 6; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(62);
                if (temp != -1 && temp == t)
                {
                    VerificationTextGenerator();
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            //GetCaptchaImage(randomCode);
            HttpContext.Current.Session["Captcha"] = randomCode;
            return GetCaptchaImageNew(randomCode);
        }

    }
}