using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;

namespace PopUp_2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        async Task SaveDataToFileAsync(string fileName, string data)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string fullPath = Path.Combine(path, fileName);

            using (StreamWriter sw = new StreamWriter(fullPath, true))
            {
                await sw.WriteAsync(data).ConfigureAwait(false);
            }
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string fullPath = Path.Combine(path, "MultiplicationTable.txt");

            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                    await DisplayAlert("Успешно", $"Файл успешно удален", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ошибка", $"Ошибка при удалении файла: {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Файл не найден", "OK");
            }
        }

        async void OnCheckButtonClicked(object sender, EventArgs e)
        {
            string[] pages = { "умножаем на 1", "умножаем на 2", "умножаем на 3", "умножаем на 4", "умножаем на 5", "умножаем на 6", "умножаем на 7", "умножаем на 8", "умножаем на 9" };
            string page = await DisplayActionSheet("Выбери множитель", "Отмена", null, pages);

            string input = await DisplayPromptAsync("Введи число от 1 до 10", "");

            if (int.TryParse(input, out int number) && number >= 1 && number <= 10)
            {
                int multiplier = Array.IndexOf(pages, page) + 1;
                int result = number * multiplier;
                await DisplayAlert("Ответ", $"{number} x {multiplier} = {result}", "OK");

                await SaveDataToFileAsync("MultiplicationTable.txt", $"{number} x {multiplier} = {result}\n");
            }
            else
            {
                await DisplayAlert("Ошибка", "Введи число от 1 до 10", "OK");
                return; 
            }
            bool answer = await DisplayAlert("Повторить?", "Хочешь повторить тест?", "Да", "Нет");
            if (answer)
            {
                OnCheckButtonClicked(sender, e);
            }
        }

        async void OnShowMultiplicationTableResults(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string fullPath = Path.Combine(path, "MultiplicationTable.txt");

            if (File.Exists(fullPath))
            {
                using (StreamReader sr = new StreamReader(fullPath))
                {
                    string content = await sr.ReadToEndAsync();
                    await DisplayAlert("Результаты", content, "OK");
                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Файл с результатами не найден", "OK");
            }
        }

    }
}