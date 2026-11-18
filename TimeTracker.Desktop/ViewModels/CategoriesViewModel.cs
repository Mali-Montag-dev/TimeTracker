using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenTK.Audio.OpenAL;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Xml.Linq;
using TimeTracker.Desktop.Services;
using TimeTracker.DTO;

namespace TimeTracker.Desktop.ViewModels
{
    public partial class CategoriesViewModel : ObservableObject
    {
        public ObservableCollection<CategoryDto> Categories { get; } = new ObservableCollection<CategoryDto>();

        public string[] ColorOptions { get; } =
        {
            "#4CAF50", "#2196F3", "#9C27B0", "#FF9800",
            "#F44336", "#00BCD4", "#795548", "#607D8B"
        };

        // ===== שדות הטופס =====
        [ObservableProperty] private string _name = string.Empty;
        [ObservableProperty] private string _selectedColor = "#4CAF50";
        [ObservableProperty] private bool _isProductive = true;

        // null = קטגוריה חדשה, מספר = עורכים קטגוריה קיימת
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsEditing))]
        [NotifyPropertyChangedFor(nameof(FormTitle))]
        [NotifyPropertyChangedFor(nameof(SaveButtonText))]
        private int? _editingId;

        // ===== שגיאות =====
        [ObservableProperty] private string _errorMessage = string.Empty;
        [ObservableProperty] private bool _hasError;

        // ===== Properties מחושבים =====
        public bool IsEditing => EditingId != null;
        public string FormTitle => IsEditing ? "עריכת קטגוריה" : "קטגוריה חדשה";
        public string SaveButtonText => IsEditing ? "שמירה" : "הוספה";

        // ===== פקודות =====

        [RelayCommand]
        private async Task LoadAsync()
        {
            await RunSafeAsync(async () =>
            {
                var list = await App.Api.GetCategoriesAsync();
                Categories.Clear();
                foreach (var category in list)
                    Categories.Add(category);
            });
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            await RunSafeAsync(async () =>
            {
                var dto = new CategoryDto
                {
                    Id = EditingId ?? 0,
                    Name = Name,
                    Color = SelectedColor,
                    IsProductive = IsProductive
                };

                if (IsEditing)
                    await App.Api.UpdateCategoryAsync(dto);
                else
                    await App.Api.CreateCategoryAsync(dto);

                ClearForm();
                await LoadAsync();
            });
        }

        [RelayCommand]
        private void Edit(CategoryDto category)
        {
            EditingId = category.Id;
            Name = category.Name;
            SelectedColor = category.Color;
            IsProductive = category.IsProductive;
        }

        [RelayCommand]
        private async Task DeleteAsync(CategoryDto category)
        {
            var confirm = new Wpf.Ui.Controls.MessageBox
            {
                Title = "מחיקת קטגוריה",
                Content = $"למחוק את הקטגוריה \"{category.Name}\"?\nהתוכנות שבה יעברו ל\"ללא קטגוריה\".",
                PrimaryButtonText = "מחיקה",
                CloseButtonText = "ביטול",
                FlowDirection = FlowDirection.RightToLeft
            };

            var result = await confirm.ShowDialogAsync();
            if (result != Wpf.Ui.Controls.MessageBoxResult.Primary)
                return;

            await RunSafeAsync(async () =>
            {
                await App.Api.DeleteCategoryAsync(category.Id);
                if (EditingId == category.Id)
                    ClearForm();
                await LoadAsync();
            });
        }

        [RelayCommand]
        private void Cancel() => ClearForm();

        // ===== עזר =====

        private void ClearForm()
        {
            EditingId = null;
            Name = string.Empty;
            SelectedColor = ColorOptions[0];
            IsProductive = true;
        }

        private async Task RunSafeAsync(Func<Task> action)
        {
            try
            {
                HasError = false;
                await action();
            }
            catch (ApiException ex)
            {
                ErrorMessage = ex.Message;
                HasError = true;
            }
            catch (HttpRequestException)
            {
                ErrorMessage = "ה-API לא זמין. ודאי שהוא פועל.";
                HasError = true;
            }
        }
    }
}