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

            Thread.Sleep(2000);
        }
        public static void DailyAdd()
        {
            Console.WriteLine("============ Günlük Ekle ============");

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
        public static void DailyGetAll()
        {
            Console.WriteLine("============ Günlükleri Listele ============");
            List<Diary> diaries = DiaryController.GetAll();

            if (diaries.Count > 0)
            {
                foreach (Diary diary in diaries)
                {
                    Console.WriteLine(diary.DateCreated.ToString("dd MMMM yyyy"));
                    Console.WriteLine(diary.Name);
                    Console.WriteLine("---------------------");
                }
                Console.WriteLine("Devam Etmek için her hangi tuşa basınız.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Gösterilecek Günlük Bulunmuyor.");
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
        public static void Exit()
        {
            Console.WriteLine("============ Güle Güle ============");
            Thread.Sleep(1000);
            Environment.Exit(0);
        }
    }
}
