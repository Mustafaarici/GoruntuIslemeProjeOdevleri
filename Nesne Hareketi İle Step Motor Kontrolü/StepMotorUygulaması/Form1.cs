using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging.Filters;
using AForge.Vision;
using AForge.Vision.Motion;
using AForge.Imaging.Filters;
using AForge.Imaging;

namespace StepMotorUygulaması
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int red,green,blue;
        int X_Piksel, Y_Piksel, islemgören_x, islemgören_y;
        BlobCounter tanımlanannesne = new BlobCounter();
        private FilterInfoCollection webcamsayisi;
        private VideoCaptureDevice kamera;
        private void kullanilacakcihaz_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap image = (Bitmap)eventArgs.Frame.Clone();
            Bitmap image1 = (Bitmap)eventArgs.Frame.Clone();
            image = new Mirror(false, true).Apply(image);
            görüntü_box.Image = image;
            EuclideanColorFiltering filter = new EuclideanColorFiltering();
            filter.CenterColor = new RGB(Color.FromArgb(red, green, blue)); // Algılanacak Renk ve merkez noktası bulunur.
            filter.Radius = 80;
            filter.ApplyInPlace(image1);//Filitre Çalıştırılır.             
            cevreal(image1);// Algilanan rengi Çevrçevelemek veya hedeflemek için gerekli Method.
            BlobsFiltering filter2 = new BlobsFiltering( ); // Belirli Piksel Altındaki Görüntüler Alınmadı.
              filter2.CoupledSizeFiltering = true;
              filter2.MinWidth = 100;
              filter2.MinHeight = 60;
              filter2.ApplyInPlace(image1);
              image1 = new Mirror(false, true).Apply(image1);
            pictureBox2.Image = image1;
        }
        private void cevreal(Bitmap image)// Algilanan rengi Çevrçevelemek veya hedeflemek için gerekli Method.
        {
            tanımlanannesne.MinWidth = 10; // İstenilen boyutlardaki nesneleri algılamak için kullanılır.
            tanımlanannesne.MinHeight = 10;
            tanımlanannesne.FilterBlobs = true;
            tanımlanannesne.ObjectsOrder = ObjectsOrder.Size;
            Grayscale grayFilter = new Grayscale(0.2125, 0.7154, 0.0721);
            image = new Mirror(false, true).Apply(image);
            Bitmap grayImage = grayFilter.Apply(image);
            tanımlanannesne.ProcessImage(grayImage);
            Rectangle[] rects = tanımlanannesne.GetObjectsRectangles();
            foreach (Rectangle recs in rects)
            {
                if (rects.Length > 0)
                {
                    Rectangle objectRect = rects[0];
                    Graphics g = görüntü_box.CreateGraphics();
                    Graphics g2 = pictureBox2.CreateGraphics();
                    using (Pen pen = new Pen(Color.FromArgb(red, green, blue), 2))
                    {
                        g.DrawEllipse(pen, objectRect); // Elips Şeklinde Çizim.
                        g2.DrawRectangle(pen, objectRect); // Dikdörtgen Şeklinde Çizim.
                    }
                    X_Piksel = objectRect.X + (objectRect.Width / 2); //Dikdörtgenin Koordinatlari alınır.
                    Y_Piksel = objectRect.Y + (objectRect.Height / 2);//Dikdörtgenin Koordinatlari alınır.
                    islemgören_x = objectRect.X + (objectRect.Width / 2); //Dikdörtgenin Koordinatlari alınır.
                    islemgören_y = objectRect.Y + (objectRect.Height / 2);//Dikdörtgenin Koordinatlari alınır.
                    g.DrawString(X_Piksel.ToString() + "X" + Y_Piksel.ToString(), new Font("Arial", 12), Brushes.Red, new System.Drawing.Point(250, 1)); // Çizim yapılır.
                    g2.DrawString(islemgören_x.ToString() + "X" + islemgören_y.ToString(), new Font("Arial", 12), Brushes.Red, new System.Drawing.Point(250, 1)); // Çizim yapılır.
                    g.Dispose();
                    g2.Dispose();

                }
                
            }
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            webcamsayisi = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo videocapturedevice in webcamsayisi)
            {
                kameraBox.Items.Add(videocapturedevice.Name);
            }
            kameraBox.SelectedIndex = 0;
            timer1.Start(); // Renk seçimini görebilmek için kullanılmıştır.
             Control.CheckForIllegalCrossThreadCalls = false;
             for (int i = 0; i < System.IO.Ports.SerialPort.GetPortNames().Length; i++)
             {
                 comboBox1.Items.Add(System.IO.Ports.SerialPort.GetPortNames()[i]); // Seriportları combobox'a aktarır.
             }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            kamera = new VideoCaptureDevice(webcamsayisi[kameraBox.SelectedIndex].MonikerString);
            kamera.NewFrame += new NewFrameEventHandler(kullanilacakcihaz_NewFrame);
            kamera.Start(); // Kamera görüntüsü başlatılır.
        }

       

        private void timer1_Tick(object sender, EventArgs e)
        {
            red = trackBar1.Value; // Kırmızı değeri alınır.
            green = trackBar2.Value; // Yeşil değeri alınır.
            blue = trackBar3.Value;  // Mavi değeri alınır.
            Color renk = Color.FromArgb(red, green, blue); // Alınan değerler ile yeni bir renk oluşturulur.
            pictureBox1.BackColor = renk; // Trackbar değerlerine göre arkaplan rengi belirlendi.
         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox1.Text; // Seri Port Combobox'tan Seçilir.
                serialPort1.Open(); // Seri Port açılır.
                MessageBox.Show("Bağlantı Kuruldu.");
            }
            catch
            {
                MessageBox.Show("Bağlantı Hatası.");
            }
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer3.Start(); 
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            if(tanımlanannesne.ObjectsCount>0) // Algılanan nesne varsa işlem yap.
            {
                if (islemgören_x > 370 )
                {
                    serialPort1.Write("a");
                    label7.Text = "Hareketli Nesne Tespit Edildi.";
                }
                if (islemgören_x < 290 )
                {
                    serialPort1.Write("c");
                    label7.Text = "Hareketli Nesne Tespit Edildi.";
                }
                if (islemgören_x > 300 && islemgören_x < 360)
                {
                    label7.Text = "Nesne Belirlendi.";
                    serialPort1.Write("d");
                }
            }
            else
            {
                serialPort1.Write("b");
                label7.Text = "Başlangıç Pozisyonuna Geçiliyor.";
            }
        }
    }
}
