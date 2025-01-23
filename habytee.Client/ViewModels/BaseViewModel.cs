using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace habytee.Client.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    public BaseViewModel? ParentViewModel { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public virtual bool IsValid
    {
        get
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(this, null, null);
            bool isValid = Validator.TryValidateObject(this, validationContext, validationResults, true);
            
            return isValid;
        }
    }

    public event Action? FormSubmitRequested;
    public void OnFormSubmitRequested()
    {
        FormSubmitRequested?.Invoke();
    }
}
