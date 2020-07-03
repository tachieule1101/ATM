using System;
using System.Collections;
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
            //Khởi tạo các ds
            List<Admin> ds_admin = new List<Admin>();
            List<TheTu> ds_thetu = new List<TheTu>();
            List<ID> ds_id = new List<ID>();
            List<LSID> ds_lsid = new List<LSID>();

            //Nạp dữ liệu
            DocFile(ds_admin, ds_thetu, ds_id, ds_lsid);
            //Xuat(ds_admin, ds_thetu, ds_id, ds_lsid);
            //Chọn loại tài khoản
            int loaiTaiKhoan;

            do
            {
                Console.Clear();
                Console.Write("Admin nhap 1, User nhap 2: ");
                int.TryParse(Console.ReadLine(), out loaiTaiKhoan);
            } while (loaiTaiKhoan != 1 && loaiTaiKhoan != 2);

            //Chương trình
            kTra_DangNhap(ds_admin, ds_thetu, loaiTaiKhoan);
            if (loaiTaiKhoan == 1)
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
        static void kTra_DangNhap(List<Admin> a, List<TheTu> b, int loai) //Nếu loại = 1 là Admin, = 2 là User
        {
            string taiKhoan, matKhau;
            int kT = 0, lanNhap = 0, hienThi, dong = 0;

            //Kiểm tra của Admin
            if (loai == 1)
            {
                //Kiểm tra theo kT đúng thì kT thành 1 và thoát vòng lập
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
                        if (string.Compare(a[i].user, taiKhoan) == 0 && string.Compare(a[i].pass, matKhau) == 0)
                        {
                            kT++;
                        }
                    }
                } while (kT == 0);
            }
            //Kiểm tra của User
            else
            {

                do
                {
                    if (lanNhap == 3)
                    {
                        lanNhap = 0;
                    }

                    hienThi = 0;
                    Console.Clear();
                    logo_DangNhapUser();

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("{0,15}{1}", "", "User: ");
                    Console.ResetColor();
                    taiKhoan = Console.ReadLine();

                    for (int i = 0; i < b.Count; i++)
                    {
                        if (string.Compare(b[i].id, taiKhoan) == 0) //Kiểm tra tài khoản tồn tại hay không
                        {
                            if (string.Compare(b[i].tinhTrang, "1") == 0) //Rồi kiểm tra trình trạng tài khoản
                            {
                                dong = i; //Lấy dòng của TK
                                do
                                {
                                    Console.Clear();
                                    logo_DangNhapUser();

                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.Write("{0,15}{1}", "", "User: ");
                                    Console.ResetColor();
                                    Console.WriteLine(taiKhoan);

                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.Write("{0,15}{1}", "", "Pin: ");
                                    Console.ResetColor();
                                    matKhau = Console.ReadLine();

                                    if (string.Compare(b[i].maPin, matKhau) == 0) //Sau cùng kiểm tra mật khẩu
                                    {
                                        kT++; //Trường hợp thỏa tất cả điều kiện
                                    }
                                    else
                                    {
                                        lanNhap++; //Trường hợp sai mật khẩu
                                        if (lanNhap != 3)
                                        {
                                            Console.WriteLine("Mat khau sai. Nhap lai!!!");
                                            Console.ReadKey();
                                        }
                                    }
                                } while (kT == 0 && lanNhap != 3);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Tai khoan nay hien dang bi khoa!!!");
                                Console.ResetColor();
                                Console.ReadKey();
                                hienThi++;
                            }
                        }

                    }

                    if (lanNhap == 3) //Khóa TK
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Khoa tai khoan!!!");
                        Console.ResetColor();
                        Console.ReadKey();
                        hienThi++;

                        //Sửa trong file
                        using (StreamWriter sw = new StreamWriter(@"D:\Do_An\Do_An2\TheTu.txt"))
                        {
                            sw.WriteLine(b.Count);
                            for (int i = 0; i < b.Count; i++)
                            {
                                if (i == dong)
                                {
                                    sw.WriteLine("0#" + b[i].id + "#" + b[i].maPin);
                                }
                                else
                                {
                                    sw.WriteLine(b[i].tinhTrang + "#" + b[i].id + "#" + b[i].maPin);
                                }
                            }
                        }

                        //Sửa trong chương trình  
                        b[dong].tinhTrang = "0";
                    }

                    if (kT == 0 && hienThi == 0) //Trường hợp sai TK hoặc MK
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Tai khoan hoac mat khau sai. Nhap lai!!!");
                        Console.ResetColor();
                        Console.ReadKey();
                    }
                } while (kT == 0);
            }
        }

        //Thành phần của Admin
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
        static void XemDS(List<Admin> a, List<TheTu> b, List<ID> c, List<LSID> d)
        {
            int chon;
            do
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

                Console.WriteLine("1.Quay lai");
                Console.WriteLine("2.Thoat");
                Console.Write("Chon chac nang: ");
                int.TryParse(Console.ReadLine(), out chon);
                if (chon == 1)
                {
                    Menu_Admin(a, b, c, d);
                }
                if (chon == 2)
                {
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("");
                        Console.WriteLine("{0,5}{1}", "", "BAN MUON THOAT?");
                        Console.WriteLine("{0}{1,10}{2}", "1.Co", "", "2.Quay lai");
                        int.TryParse(Console.ReadLine(), out chon);
                    } while (chon != 1 && chon != 2);
                    if (chon == 1)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        XemDS(a, b, c, d);
                    }
                }
            } while (chon != 1 && chon != 2);
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

        //4_Admin
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
        static void MoKhoa(List<Admin> a, List<TheTu> b, List<ID> c, List<LSID> d)
        {
            int chon, kT = 0, dong =0;
            string ID;
            List<TheTu> khoa = new List<TheTu>();
            do
            {
                Console.Clear();
                logo_MoKhoa();

                Console.WriteLine("{0,14}{1}", "", "DANH SACH TAI KHOAN BI KHOA\n\n");
                for (int i = 0, j = 0; i < b.Count; i++)
                {
                    if (string.Compare(b[i].tinhTrang, "0") == 0)
                    {
                        Console.WriteLine("{0,10}{1}{2}{3}", "", "----------Tai khoan thu: ", ++j, "----------");
                        Console.Write("ID: {0}", b[i].id);
                        Console.WriteLine("\tMa PIN: {0}", b[i].maPin);
                        Console.WriteLine("");
                        khoa.Add(b[i]);
                    }
                }

                Console.WriteLine("1.Mo khoa");
                Console.WriteLine("2.Quay lai");
                Console.WriteLine("3.Thoat");
                Console.Write("Chon chac nang: ");
                int.TryParse(Console.ReadLine(), out chon);
                if (chon == 1)
                {

                    do
                    {
                        Console.Clear();
                        logo_MoKhoa();

                        Console.WriteLine("{0,14}{1}", "", "DANH SACH TAI KHOAN BI KHOA\n\n");
                        for (int i = 0; i < khoa.Count; i++)
                        {

                            Console.WriteLine("{0,10}{1}{2}{3}", "", "----------Tai khoan thu: ", i + 1, "----------");
                            Console.Write("ID: {0}", khoa[i].id);
                            Console.WriteLine("\tMa PIN: {0}", khoa[i].maPin);
                            Console.WriteLine("");
                        }
                        Console.Write("Nhap ID muon mo khoa: ");
                        ID = Console.ReadLine();

                        for (int i = 0; i < khoa.Count; i++)
                        {
                            if (string.Compare(ID, khoa[i].id) == 0)
                            {
                                for (int j = 0; j < b.Count; j++)
                                {
                                    if (string.Compare(ID, b[j].id) == 0)
                                    {
                                        dong = j;
                                    }                                
                                }
                                //Sửa trong file
                                using (StreamWriter sw = new StreamWriter(@"D:\Do_An\Do_An2\TheTu.txt"))
                                {
                                    sw.WriteLine(b.Count);
                                    for (int j = 0; j < b.Count; j++)
                                    {
                                        if (j == dong)
                                        {
                                            sw.WriteLine("1#" + b[j].id + "#" + b[j].maPin);
                                        }
                                        else
                                        {
                                            sw.WriteLine(b[j].tinhTrang + "#" + b[j].id + "#" + b[j].maPin);
                                        }
                                    }
                                }

                                //Sửa trong chương trình  
                                b[dong].tinhTrang = "1";
                                kT++;
                            }
                        }
                        if (kT == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Nhap sai ID. Nhap lai!!!");
                            Console.ResetColor();
                            Console.ReadKey();
                        }
                    } while (kT == 0);
                    if (kT != 0)
                    {
                        MoKhoa(a, b, c, d);
                    }
                }
                if (chon == 2)
                {
                    Menu_Admin(a, b, c, d);
                }
                if (chon == 3)
                {
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("");
                        Console.WriteLine("{0,5}{1}", "", "BAN MUON THOAT?");
                        Console.WriteLine("{0}{1,10}{2}", "1.Co", "", "2.Quay lai");
                        int.TryParse(Console.ReadLine(), out chon);
                    } while (chon != 1 && chon != 2);
                    if (chon == 1)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        MoKhoa(a, b, c, d);
                    }
                }
            } while (chon != 1 && chon != 2);
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
                XemDS(a, b, c, d);
            }
            if (chucNang == 2)
            {
                
            }
            if (chucNang == 3)
            {
                
            }
            if (chucNang == 4)
            {
                MoKhoa(a, b, c, d);
            }
            if (chucNang == 5)
            {
                kT_Thoat(a, b, c, d, 1);
            }
        }


        //Thành phần của User
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
        static void Xem_TT(List<Admin> a, List<TheTu> b, List<ID> c, List<LSID> d)
        {
            int chon;
            do
            {
                Console.Clear();
                logo_XemTT();

                for (int i = 0; i < c.Count; i++)
                {
                    Console.Write("\tID: {0}", c[i].id +"\n");
                    Console.Write("\tTen: {0}", c[i].ten + "\n");
                    Console.Write("\tTien Te: {0}", c[i].tienTe + "\n");
                    Console.Write("\tSo Du: {0}", c[i].soDu + "\n");
                }

                Console.WriteLine("1.Quay lai");
                Console.WriteLine("2.Thoat");
                Console.Write("Chon chac nang: ");
                int.TryParse(Console.ReadLine(), out chon);
                if (chon == 1)
                {
                    Menu_User(a, b, c, d);
                }
                if (chon == 2)
                {
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("");
                        Console.WriteLine("{0,5}{1}", "", "BAN MUON THOAT?");
                        Console.WriteLine("{0}{1,10}{2}", "1.Co", "", "2.Quay lai");
                        int.TryParse(Console.ReadLine(), out chon);
                    } while (chon != 1 && chon != 2);
                    if (chon == 1)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Xem_TT(a, b, c, d);
                    }
                }
            } while (chon != 1 && chon != 2);
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
        static void Rut_Tien(List<Admin> a, List<TheTu> b, List<ID> c, List<LSID> d)
        {
            Console.Clear();
            logo_Rut();
        }
        static void Chuyen_Tien(List<Admin> a, List<TheTu> b, List<ID> c, List<LSID> d)
        {
            Console.Clear();
            logo_Chuyen();
        }
        static void XemND_GiaoDich(List<Admin> a, List<TheTu> b, List<ID> c, List<LSID> d)
        {
            Console.Clear();
            logo_GiaoDich();
            //dieu kien

            //chuc nang

        }
        static void Doi_MaPin(List<Admin> a, List<TheTu> b, List<ID> c, List<LSID> d)
        {
            Console.Clear();
            logo_DoiPin();

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

            if (chucNang == 1)
            {
                Xem_TT(a, b, c, d);
            }
            if (chucNang == 2)
            {
                Rut_Tien(a, b, c, d);
            }
            if (chucNang == 3)
            {
                Chuyen_Tien(a, b, c, d);
            }
            if (chucNang == 4)
            {
                XemND_GiaoDich(a, b, c, d);
            }
            if (chucNang == 5)
            {
                Doi_MaPin(a, b, c, d);
            }
            if (chucNang == 6)
            {
                kT_Thoat(a, b, c, d, 2);
            }
        }

        //Kiểm tra thoát
        static void kT_Thoat(List<Admin> a, List<TheTu> b, List<ID> c, List<LSID> d, int loai)
        {
            int kTThoat;

            do
            {
                Console.Clear();
                Console.WriteLine("{0,5}{1}", "", "BAN MUON THOAT?");
                Console.WriteLine("{0}{1,10}{2}", "1.Co", "", "2.Quay lai");
                int.TryParse(Console.ReadLine(), out kTThoat);
            } while (kTThoat != 1 && kTThoat != 2);
            if (kTThoat == 1)
            {
                Environment.Exit(0);
            }
            else
            {
                if (loai == 1)
                {
                    Menu_Admin(a, b, c, d);
                }
                if (loai == 2)
                {
                    Menu_User(a, b, c, d);
                }
            }
        }

        //Xuất dữ liệu
        static void Xuat(List<Admin> a, List<TheTu> b, List<ID> c, List<LSID> d)
        {
            Console.WriteLine("Admin");
            for (int i = 0; i < a.Count; i++)
            {
                Console.WriteLine(a[i].user);
                Console.WriteLine(a[i].pass);
            }
            Console.WriteLine("TheTu");
            for (int i = 0; i < b.Count; i++)
            {
                Console.WriteLine(b[i].tinhTrang);
                Console.WriteLine(b[i].id);
                Console.WriteLine(b[i].maPin);
            }
            Console.WriteLine("ID");
            for (int i = 0; i < c.Count; i++)
            {
                Console.WriteLine(c[i].id);
                Console.WriteLine(c[i].ten);
                Console.WriteLine(c[i].soDu);
                Console.WriteLine(c[i].tienTe);
            }
            Console.WriteLine("LSID");
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
            string file1 = "Admin", file2 = "TheTu", file3 = "ID";
            string dauCach = "#";
            //Đọc file Admin
            try
            {
                StreamReader sr = new StreamReader(file1 + ".txt");
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
                sr.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //Đọc file TheTu và những file còn lại
            try
            {
                //File TheTu
                StreamReader srTheTu = new StreamReader(file2 + ".txt");
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
                srTheTu.Close();

                for (int i = 0; i < b.Count; i++)
                {
                    //File ID
                    file3 = b[i].id;

                    StreamReader srID = new StreamReader( file3 + ".txt");

                    ID id = new ID();
                    line = srID.ReadLine();
                    string[] a2 = line.Split(a1, StringSplitOptions.RemoveEmptyEntries);

                    id.id = a2[0];
                    id.ten = a2[1];
                    int.TryParse(a2[2], out id.soDu);
                    id.tienTe = a2[3];

                    c.Add(id);
                    srID.Close();

                    //File LSID
                    StreamReader srLSID = new StreamReader(file3 + ".txt");

                    LSID lsid = new LSID();
                    line = srLSID.ReadLine();
                    string[] a3 = line.Split(a1, StringSplitOptions.RemoveEmptyEntries);

                    lsid.id = a3[0];
                    lsid.loaiGD = a3[1];
                    int.TryParse(a3[2], out lsid.soTien);
                    lsid.tG = a3[3];

                    d.Add(lsid);
                    srLSID.Close();
                }
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

