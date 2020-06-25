using System;
using System.Collections;
using System;
using System.Runtime.CompilerServices;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Threading;

namespace Do_An2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Chọn loại tài khoản
            string loaiTaiKhoan;
            Console.Write("Admin nhap 1, User nhap 2: ");
            loaiTaiKhoan = Console.ReadLine();


            //Khởi tạo các ds
            List<Admin> ds_admin = new List<Admin>();
            List<TheTu> ds_thetu = new List<TheTu>();
            List<ID> ds_id = new List<ID>();
            List<LSID> ds_lsid = new List<LSID>();

            //Chương trình
            DocFile(ds_admin, ds_thetu, ds_id, ds_lsid);
            kTra_DangNhap(ds_admin, ds_thetu, loaiTaiKhoan);
            if (loaiTaiKhoan.Equals("1"))
            {
                Menu_Admin(ds_admin, ds_thetu, ds_id, ds_lsid);
            }
            else
            {
                Menu_User(ds_admin, ds_thetu, ds_id, ds_lsid);
            }
            Console.ReadKey();
        }

        //Kiểm tra đăng nhập cho cả Admin và User
        static void kTra_DangNhap(List<Admin> a, List<TheTu> b, string loai) //Nếu loại = 1 là Admin, = 2 là User
        {
            string taiKhoan, matKhau;
            int kT = 0, lanNhap = 0, sai, viTri;

            if (loai.Equals("1"))
            {
                do
                {
                    Console.Clear();
                    logo_DangNhapAdmin();

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("{0,15}{1}", "", "User: ");
                    Console.ResetColor();
                    taiKhoan = Console.ReadLine();

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("{0,15}{1}", "", "Pin: ");
                    Console.ResetColor();
                    matKhau = Console.ReadLine();

                    for (int i = 0; i < a.Count; i++)
                    {
                        if (a[i].user == taiKhoan && a[i].pass == matKhau)
                        {
                            kT++;
                        }
                    }
                } while (kT == 0);
            }
            else
            {
                do
                {
                    viTri = 0;
                    sai = 0;

                    Console.Clear();
                    logo_DangNhapUser();

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("{0,15}{1}", "", "User: ");
                    Console.ResetColor();
                    taiKhoan = Console.ReadLine();

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("{0,15}{1}", "", "Pin: ");
                    Console.ResetColor();
                    matKhau = Console.ReadLine();

                    for (int i = 0; i < b.Count; i++)
                    {
                        viTri = i;
                        if (b[i].id == taiKhoan)
                        {
                            if (string.Compare(b[i].tinhTrang, "1") == 0)
                            {
                                do
                                {
                                    Console.Clear();
                                    logo_DangNhapUser();

                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.Write("{0,15}{1}{2}", "", "User: ", b[i].id);

                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.Write("{0,15}{1}", "", "Pin: ");
                                    Console.ResetColor();
                                    matKhau = Console.ReadLine();
                                    if (b[i].maPin == matKhau)
                                    {
                                        kT++;
                                    }
                                    else
                                    {
                                        lanNhap++;
                                        if (lanNhap == 3)
                                        {
                                            b[i].tinhTrang = "0";
                                            Console.Write("Khoa tai khoan!!!");
                                        }
                                    }
                                } while (b[i].maPin != matKhau && lanNhap < 3);
                            }
                            if (string.Compare(b[i].tinhTrang, "0") == 0)
                            {
                                Console.WriteLine("Tai khoan nay dang bi khoa!!!");
                                Console.ReadKey();
                                break;
                            }
                        }
                    }

                    if (viTri == b.Count)
                    {
                        Console.Write("Tai khoan hoac mat khau sai, nhap lai!!!");
                        Console.ReadKey();
                    }
                } while (kT == 0);

            }
        }

        //Admin
        static void logo_DangNhapAdmin()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.Write("{0,15}{1}", "", "*");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("    DANG NHAP ADMIN    ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*");
            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.ResetColor();
        }

        //1_Admin
        static void logo_XemDS()
        {
            ConsoleColor foreground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.Write("{0,15}{1}", "", "*");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  DANH SACH TAI KHOAN  ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*");
            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.ResetColor();
        }
        static void XemDS(List<TheTu> b)
        {
            Console.Clear();
            logo_XemDS();

            for (int i = 0; i < b.Count; i++)
            {
                Console.WriteLine("{0,10}{1}{2}{3}", "", "----------Tai khoan thu: ", i + 1, "----------");

                Console.Write("ID: {0}", b[i].id);
                Console.Write("\tMa PIN: {0}", b[i].maPin);
                if (string.Compare(b[i].tinhTrang, "0") == 0)
                {
                    Console.WriteLine("\tTinh trang: KHOA");
                }
                else
                {
                    Console.WriteLine("");
                }
            }
        }

        static void logo_Them()
        {
            ConsoleColor foreground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.Write("{0,15}{1}", "", "*");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("    THEM TAI KHOAN     ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*");
            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.ResetColor();
        }
        static void logo_Xoa()
        {
            ConsoleColor foreground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.Write("{0,15}{1}", "", "*");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("     XOA TAI KHOAN     ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*");
            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.ResetColor();
        }
        static void logo_MoKhoa()
        {
            ConsoleColor foreground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.Write("{0,15}{1}", "", "*");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("   MO KHOA TAI KHOAN   ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*");
            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.ResetColor();
        }

        static void Menu_Admin(List<Admin> a, List<TheTu> b, List<ID> c, List<LSID> d)
        {
            int chucNang;

            do
            {
                Console.Clear();
                ConsoleColor foreground = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine("");
                Console.WriteLine("{0,12}{1}", "", "*************MENU**************");
                Console.WriteLine("{0,15}{1}", "", "1.Xem danh sach tai khoan");
                Console.WriteLine("{0,15}{1}", "", "2.Them tai khoan");
                Console.WriteLine("{0,15}{1}", "", "3.Xoa tai khoan");
                Console.WriteLine("{0,15}{1}", "", "4.Mo khoa tai khoan");
                Console.WriteLine("{0,15}{1}", "", "5.Thoat");
                Console.WriteLine("{0,12}{1}", "", "*******************************");
                Console.ResetColor();
                Console.Write("Chon chac nang: ");
                int.TryParse(Console.ReadLine(), out chucNang);
            } while (chucNang < 1 || chucNang > 5);

            if (chucNang == 1)
            {
                XemDS(b);
            }
            if (chucNang == 5)
            {
                kT_ThoatAdmin(a, b, c, d);
            }
        }


        //User
        static void logo_DangNhapUser()
        {
            ConsoleColor foreground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.Write("{0,15}{1}", "", "*");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("    DANG NHAP USER     ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*");
            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.ResetColor();
        }
        static void logo_XemTT()
        {
            ConsoleColor foreground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.Write("{0,15}{1}", "", "*");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("     XEM THONG TIN     ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*");
            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.ResetColor();
        }
        static void logo_Rut()
        {
            ConsoleColor foreground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.Write("{0,15}{1}", "", "*");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("       RUT TIEN        ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*");
            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.ResetColor();
        }
        static void logo_Chuyen()
        {
            ConsoleColor foreground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.Write("{0,15}{1}", "", "*");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("      CHUYEN TIEN      ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*");
            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.ResetColor();
        }
        static void logo_GiaoDich()
        {
            ConsoleColor foreground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.Write("{0,15}{1}", "", "*");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("     XEM GIAO DICH     ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*");
            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.ResetColor();
        }
        static void logo_DoiPin()
        {
            ConsoleColor foreground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.Write("{0,15}{1}", "", "*");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("      DOI MA PIN       ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*");
            Console.WriteLine("{0,15}{1}", "", "*************************");
            Console.ResetColor();
        }

        static void Menu_User(List<Admin> a, List<TheTu> b, List<ID> c, List<LSID> d)
        {
            int chucNang;
            do
            {
                Console.Clear();
                ConsoleColor foreground = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine("");
                Console.WriteLine("{0,12}{1}", "", "*************MENU**************");
                Console.WriteLine("{0,15}{1}", "", "1.Xem thong tin tai khoan");
                Console.WriteLine("{0,15}{1}", "", "2.Rut tien");
                Console.WriteLine("{0,15}{1}", "", "3.Chuyen tien");
                Console.WriteLine("{0,15}{1}", "", "4.Xem noi dung giao dich");
                Console.WriteLine("{0,15}{1}", "", "5.Doi ma Pin");
                Console.WriteLine("{0,15}{1}", "", "6.Thoat");
                Console.WriteLine("{0,12}{1}", "", "*******************************");
                Console.ResetColor();
                Console.Write("Chon chac nang: ");
                int.TryParse(Console.ReadLine(), out chucNang);
            } while (chucNang < 1 || chucNang > 6);

            if (chucNang == 6)
            {
                kT_ThoatUser(a, b, c, d);
            }
        }


        //Kiểm tra thoát
        static void kT_ThoatAdmin(List<Admin> a, List<TheTu> b, List<ID> c, List<LSID> d)
        {
            int kTThoat;

            do
            {
                Console.Clear();
                Console.WriteLine("{0,5}{1}", "", "BAN MUON THOAT?");
                Console.WriteLine("{0}{1,10}{2}", "1.Co", "", "2.Quay lai");
                int.TryParse(Console.ReadLine(), out kTThoat);
            } while (kTThoat < 1 || kTThoat > 2);
            if (kTThoat == 1)
            {
                Environment.Exit(0);
            }
            else
            {
                Menu_Admin(a, b, c, d);
            }
        }
        static void kT_ThoatUser(List<Admin> a, List<TheTu> b, List<ID> c, List<LSID> d)
        {
            int kTThoat;

            do
            {
                Console.Clear();
                Console.WriteLine("{0,5}{1}", "", "BAN MUON THOAT?");
                Console.WriteLine("{0}{1,10}{2}", "1.Co", "", "2.Quay lai");
                int.TryParse(Console.ReadLine(), out kTThoat);
            } while (kTThoat < 1 || kTThoat > 2);
            if (kTThoat == 1)
            {
                Environment.Exit(0);
            }
            else
            {
                Menu_User(a, b, c, d);
            }
        }

        //Xuất dữ liệu
        static void Xuat(List<Admin> a, List<TheTu> b, List<ID> c, List<LSID> d)
        {

            DocFile(a, b, c, d);

            for (int i = 0; i < a.Count; i++)
            {
                Console.WriteLine(a[i].user);
                Console.WriteLine(a[i].pass);
            }

            for (int i = 0; i < b.Count; i++)
            {
                Console.WriteLine(b[i].tinhTrang);
                Console.WriteLine(b[i].id);
                Console.WriteLine(b[i].maPin);
            }

            for (int i = 0; i < c.Count; i++)
            {
                Console.WriteLine(c[i].id);
                Console.WriteLine(c[i].ten);
                Console.WriteLine(c[i].soDu);
                Console.WriteLine(c[i].tienTe);
            }

            for (int i = 0; i < d.Count; i++)
            {
                Console.WriteLine(d[i].id);
                Console.WriteLine(d[i].loaiGD);
                Console.WriteLine(d[i].soTien);
                Console.WriteLine(d[i].tG);
            }

        }

        //Nạp dữ liệu từ file
        static void DocFile(List<Admin> a, List<TheTu> b, List<ID> c, List<LSID> d)
        {
            //Khai báo
            string file1 = "Admin", file2 = "TheTu", file3 = "", file4 = "";
            string dauCach = "#";

            //Đọc file Admin
            try
            {
                StreamReader sr = new StreamReader(@"D:\Do_An\Do_An2\" + file1 + ".txt");
                int soLuong;

                string[] a1 = new string[] { dauCach };
                string line;

                line = sr.ReadLine();
                int.TryParse(line, out soLuong);

                for (int i = 0; i < soLuong; i++)
                {
                    Admin admin = new Admin();
                    line = sr.ReadLine();
                    string[] a2 = line.Split(a1, StringSplitOptions.RemoveEmptyEntries);
                    admin.user = a2[0];
                    admin.pass = a2[1];

                    a.Add(admin);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //Đọc file TheTu và những file còn lại
            try
            {
                //File TheTu
                StreamReader srTheTu = new StreamReader(@"D:\Do_An\Do_An2\" + file2 + ".txt");
                int soLuong;

                string[] a1 = new string[] { dauCach };
                string line;

                line = srTheTu.ReadLine();
                int.TryParse(line, out soLuong);

                for (int i = 0; i < soLuong; i++)
                {
                    TheTu theTu = new TheTu();
                    line = srTheTu.ReadLine();
                    string[] a2 = line.Split(a1, StringSplitOptions.RemoveEmptyEntries);
                    theTu.tinhTrang = a2[0];
                    theTu.id = a2[1];
                    theTu.maPin = a2[2];

                    b.Add(theTu);
                }

                for (int i = 0; i < b.Count; i++)
                {
                    //File ID
                    file3 = b[i].id;

                    StreamReader srID = new StreamReader(@"D:\Do_An\Do_An2\ID\" + file3 + ".txt");

                    ID id = new ID();
                    line = srID.ReadLine();
                    string[] a2 = line.Split(a1, StringSplitOptions.RemoveEmptyEntries);

                    id.id = a2[0];
                    id.ten = a2[1];
                    int.TryParse(a2[2], out id.soDu);
                    id.tienTe = a2[3];

                    c.Add(id);

                    //File LSID
                    file4 = b[i].id;

                    StreamReader srLSID = new StreamReader(@"D:\Do_An\Do_An2\LSID\" + file4 + ".txt");

                    LSID lsid = new LSID();
                    line = srLSID.ReadLine();
                    string[] a3 = line.Split(a1, StringSplitOptions.RemoveEmptyEntries);

                    lsid.id = a3[0];
                    lsid.loaiGD = a3[1];
                    int.TryParse(a3[2], out lsid.soTien);
                    lsid.tG = a3[3];

                    d.Add(lsid);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

