using DailyApp.Controller;
using DailyApp.Model;

namespace DailyApp.View
{
    public static class Menu
    {
        public static void Index()
        {
            Console.Clear();
            Console.WriteLine("============ Hoşgeldiniz ============");
            Console.WriteLine("1) Günlük Ekle");
            Console.WriteLine("2) Günlükleri Listele");
            Console.WriteLine("3) Tüm Günlükleri Sil");
            Console.WriteLine("0) Çıkış");

            string? selection = Console.ReadLine();
            Console.Clear();
            ChooseOption(selection ?? "");
        }
        public static void ChooseOption(string selection)
        {
            switch (selection)
            {
                case "":
                    Console.WriteLine("Lütfen Geçerli bir sayı giriniz.");
                    break;
                case "0":
                    Exit();
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
                default:
                    Console.WriteLine("Hatalı sayı girdiniz.\nLütfen 0 ile 3 Arasında bir sayı giriniz.");
                    break;
            }
        }
        public static void DailyAdd()
        {
            Console.WriteLine("============ Günlük Ekle ============");
            
            if(!DiaryController.CheckCurrentDateHasDiary())
            {
                Console.Write("Günlük Ekleyiniz: ");

                Diary daily = new() { Name = Console.ReadLine() };

                if (DiaryController.Add(daily))
                {
                    Console.WriteLine("Başarılı Şekilde Eklendi");
                }
                else
                {
                    Console.WriteLine("Eklenirken bir hata oluştu. Yeniden deneyiniz lütfen");
                }
            }
            else
            {
                Console.WriteLine($"{DateTime.Now.ToString("dd MMMM yyyy")} Tarihi için günlük daha önceden eklendi. Lütfen Ana menüye Dönünüz.");
            }

        }
        public static void DailyGetAll()
        {
            
            List<Diary> diaries = DiaryController.GetAll();
            int count = 1;

            if (diaries.Count > 0)
            {
                foreach (Diary diary in diaries)
                {
                    Console.WriteLine("============ Günlükleri Listele ============");
                    Console.WriteLine(diary.DateCreated.ToString("dd MMMM yyyy"));
                    Console.WriteLine(diary.Name);
                    Console.WriteLine($"{(diaries.Count != count ? "(s)onraki kayıt | ": "")}(d)üzenle | (si)l | (a)na menü | Kalan Günlük: {diaries.Count - count}");
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
                if (DiaryController.RemoveAll())
                {
                    Console.WriteLine("Tüm günlükler başarı ile silindi.");
                }
                else
                {
                    Console.WriteLine("Silinirken hata oluştu yeniden deneyiniz lütfen.");
                }
            }
        }
        public static void DailyUpdate(Diary diary)
        {
            Console.Write($"\"{diary.Name}\" Günlük düzenlemek için yazınız: ");
            diary.Name = Console.ReadLine();
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
            if (DiaryController.RemoveById(id))
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
            Console.WriteLine("============ Güle Güle ============");
            Thread.Sleep(1000);
            Environment.Exit(0);
        }
    }
}
