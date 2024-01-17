using DailyApp.Controller;
using DailyApp.Model;

namespace DailyApp.View
{
    public static class Menu
    {
        public static User _user { get; set; } = null;
        public static void Index()
        {

            LogMenu();

            if (_user != null)
            {
                Console.Clear();
                Console.WriteLine("============ Hoşgeldiniz ============");
                Console.WriteLine("1) Günlük Ekle");
                Console.WriteLine("2) Günlükleri Listele");
                Console.WriteLine("3) Tüm Günlükleri Sil");
                Console.WriteLine("4) Kayıt Ara");
                Console.WriteLine("0) Çıkış");

                string? selection = Console.ReadLine();
                Console.Clear();
                ChooseOption(selection ?? "");
            }
        }
        public static void LogMenu()
        {
            if (_user == null)
            {
                Console.Clear();
                Console.WriteLine("============ Günlük Uygulaması ============");
                Console.WriteLine("1) Giriş Yap");
                Console.WriteLine("2) Kayıt Ol");
                Console.WriteLine("3) Şifremi Unuttum");
                Console.WriteLine("0) Çıkış");
                string option = Console.ReadLine();
                switch(option)
                {
                    case "1":
                        Login();
                        break;
                    case "2":
                        Register();
                        break;
                    case "3":
                        ForgetPassword();
                        break;
                    case "0":
                        Exit(); 
                        break;
                    default:
                        Console.WriteLine("default");
                        break;
                }
            }
        }
        public static void Login()
        {
            if (_user == null)
            {
                Console.Clear();
                Console.WriteLine("============ Günlük Uygulaması ============");
                Console.WriteLine("Lütfen Giriş Yapınız.");
                User user = new User();
                Console.Write("Kullanıcı Adı: ");
                user.Username = Console.ReadLine();
                Console.Write("Şifre: ");
                user.Password = Console.ReadLine();

                _user = UserController.Login(user);
                if (_user == null) 
                {
                    Console.WriteLine("Yanlış Kullanıcı adı veya şifre girdiniz");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            }            
        }
        public static void Logout()
        {
            Console.Clear();
            Console.WriteLine("============ Güle Güle ============");
            _user = null;
            Thread.Sleep(1000);
        }
        public static void Register()
        {
            User user = new User();

            Console.Clear();
            Console.WriteLine("============ Günlük Uygulaması ============");
            Console.Write("Adınızı Giriniz: ");
            user.Fullname = Console.ReadLine();
            Console.Write("Kullanıcı Adı Giriniz: ");
            user.Username = Console.ReadLine();
            Console.Write("Şifrenizi Giriniz: ");
            user.Password = Console.ReadLine();
            Console.Write("Güvenlik Sorunuzu Giriniz: ");
            user.SecurityQuestion = Console.ReadLine();
            Console.Write("Güvenlik Cevabınızı Giriniz: ");
            user.SecurityAnswer = Console.ReadLine();

            if (UserController.Register(user))
            {
                Console.WriteLine("Başarılı olarak kayıt oluştu.");
            }
            else
            {
                Console.WriteLine("Kayıt olurken hata oluştu.");
            }
        }
        public static void ForgetPassword()
        {
            Console.Clear();
            Console.WriteLine("============ Şifre Yenileme ============");
            Console.Write("Kullanıcı Adınızı Giriniz: ");
            User foundUser = UserController.GetByUsername(Console.ReadLine());
            if (foundUser != null)
            {
                Console.Write($"Güvenlik Sorusu {foundUser.SecurityQuestion}: ");

                string securityAnswer = Console.ReadLine();
                if (securityAnswer == foundUser.SecurityAnswer)
                {
                    Console.Write("Yeni Şifrenizi giriniz: ");
                    foundUser.Password = Console.ReadLine();
                    if (UserController.ForgetPassword(foundUser))
                        Console.WriteLine("Başarılı olarak şifreniz güncellendi");
                    else
                        Console.WriteLine("Şifreniz güncellenirken hata oluştu.");
                }
                else
                    Console.WriteLine("Güvenlik sorunu yanlış girdiniz.");
            }
            else
            {
                Console.WriteLine("Yanlış Kullanıcı Adı Girdiniz.");
            }
            Thread.Sleep(1000);
        }
        public static void ChooseOption(string selection)
        {
            switch (selection)
            {
                case "":
                    Console.WriteLine("Lütfen Geçerli bir sayı giriniz.");
                    break;
                case "0":
                    Logout();
                    break;
                case "1":
                    DailyAdd();
                    Thread.Sleep(2000);
                    break;
                case "2":
                    DailyGetAll();
                    break;
                case "3":
                    DailyRemoveAll();
                    break;
                case "4":
                    GetDailyByDate();
                    break;
                default:
                    Console.WriteLine("Hatalı sayı girdiniz.\nLütfen 0 ile 4 Arasında bir sayı giriniz.");
                    break;
            }
        }
        public static void DailyAdd()
        {
            Console.WriteLine("============ Günlük Ekle ============");

            if (!DiaryController.CheckCurrentDateHasDiary(_user.Id))
                Add();
            else
            {
                Console.WriteLine("Bugün günlük kaydı girdin, aynı tarihte yeni bir kayıt eklemek ister misin? (e)vet/(h)ayır");
                string option = Console.ReadLine();
                switch (option)
                {
                    case "e":
                        Add();
                        break;
                    case "h":
                        break;
                }
            }

        }
        public static void Add()
        {
            Console.Write("Günlük Ekleyiniz: ");

            Diary daily = new() { Name = Console.ReadLine(), UserId = _user.Id };

            if (DiaryController.Add(daily))
            {
                Console.WriteLine("Başarılı Şekilde Eklendi");
            }
            else
            {
                Console.WriteLine("Eklenirken bir hata oluştu. Yeniden deneyiniz lütfen");
            }
        }
        public static void DailyGetAll()
        {

            List<Diary> diaries = DiaryController.GetAll(_user.Id);
            int count = 1;

            if (diaries.Count > 0)
            {
                foreach (Diary diary in diaries)
                {
                    Console.WriteLine("============ Günlükleri Listele ============");
                    Console.WriteLine(diary.DateCreated.ToString("dd MMMM yyyy"));
                    Console.WriteLine(diary.Name);
                    Console.WriteLine($"{(diaries.Count != count ? "(s)onraki kayıt | " : "")}(d)üzenle | (si)l | (a)na menü | Kalan Günlük: {diaries.Count - count}");
                    string option = Console.ReadLine();
                    switch (option)
                    {
                        case "a":
                            return;
                        case "s":
                            Console.Clear();
                            count++;
                            continue;
                        case "d":
                            DailyUpdate(diary);
                            return;
                        case "si":
                            DailyRemoveById(diary.Id);
                            return;
                        default:
                            return;
                    }
                }
            }
            else
            {
                Console.WriteLine("Gösterilecek Günlük Bulunmuyor.");
                Thread.Sleep(1000);
            }
        }
        public static void DailyRemoveAll()
        {
            Console.WriteLine("============ Günlükleri Sil ============");
            Console.WriteLine("Tüm Günlükleri Silmek İstediğinizden Emin Misiniz ?(e/Evet, h/Hayır)");
            string option = Console.ReadLine();
            if (option == "e")
            {
                if (DiaryController.RemoveAll(_user.Id))
                {
                    Console.WriteLine("Tüm günlükler başarı ile silindi.");
                }
                else
                {
                    Console.WriteLine("Silinirken hata oluştu yeniden deneyiniz lütfen.");
                }
            }
        }
        public static void GetDailyByDate()
        {
            Console.WriteLine("============ Günlük Ara ============");
            Console.Write("Tarih Giriniz: ");
            DateTime date = DateTime.Parse(Console.ReadLine());

            List<Diary> diaries = DiaryController.GetDiariesByDate(date, _user.Id);
            foreach (Diary diary in diaries)
            {
                Console.WriteLine(diary.DateCreated.ToString("dd MMMM yyyy"));
                Console.WriteLine(diary.Name);
                Console.WriteLine("---------------------");
            }
            Thread.Sleep(2000);
        }
        public static void DailyUpdate(Diary diary)
        {
            Console.Write($"\"{diary.Name}\" Günlük düzenlemek için yazınız: ");
            diary.Name = Console.ReadLine();
            diary.UserId = _user.Id;
            if (DiaryController.Update(diary))
            {
                Console.WriteLine("Günlük Başarılı ile düzenlendi.");
            }
            else
            {
                Console.WriteLine("Günlük düzenlenirken bir hata oluştu lütfen yeniden deneyiniz.");
            }
            Thread.Sleep(1000);
        }
        public static void DailyRemoveById(int id)
        {
            if (DiaryController.RemoveById(id, _user.Id))
            {
                Console.WriteLine("Günlük Başarılı ile silindi.");
            }
            else
            {
                Console.WriteLine("Günlük silinirken bir hata oluştu lütfen yeniden deneyiniz.");
            }
            Thread.Sleep(1000);
        }
        public static void Exit()
        {
            Console.Clear();
            Console.WriteLine("============ Güle Güle ============");
            Thread.Sleep(1000);
            Environment.Exit(0);
        }
    }
}
