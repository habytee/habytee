using habytee.Client.ViewModels;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Microsoft.JSInterop;
using habytee.Client.Services;

namespace habytee.Client.Pages;

public partial class CreateNewHabit
{
    private AddHabitViewModel? addHabitViewModel;

    private RadzenTemplateForm<AddHabitNameViewModel>? addHabitNameViewModelForm;
    private RadzenTemplateForm<AddHabitReasonViewModel>? addHabitReasonViewModelForm;
    private RadzenTemplateForm<AddHabitDaysViewModel>? addHabitDaysViewModelForm;
    private RadzenTemplateForm<AddHabitAlarmViewModel>? addHabitAlarmViewModelForm;
    private RadzenTemplateForm<AddHabitEarningsViewModel>? addHabitEarningsViewModelForm;
    private string? message;

    [Inject]
    private ApiService? apiService { get; set; }

    [Inject]
    private NavigationManager? navigationManager { get; set; }

    [Inject]
    private MessageService? messageService { get; set; }

    [Inject]
    private DialogService? dialogService { get; set; }

    [Inject]
    private IJSRuntime? iJSRuntime{ get; set; }

    private void OnAddHabitNameViewModelFormSubmitRequested()
    {
        addHabitNameViewModelForm?.Submit.InvokeAsync();
    }

    private void OnAddHabitReasonViewModelFormSubmitRequested()
    {
        addHabitReasonViewModelForm?.Submit.InvokeAsync();
    }

    private void OnAddHabitDaysViewModelFormSubmitRequested()
    {
        addHabitDaysViewModelForm?.Submit.InvokeAsync();
    }

    private void OnAddHabitAlarmViewModelFormSubmitRequested()
    {
        addHabitAlarmViewModelForm?.Submit.InvokeAsync();
    }

    private void OnAddHabitEarningsViewModelFormSubmitRequested()
    {
        addHabitEarningsViewModelForm?.Submit.InvokeAsync();
    }

    private void HandleMessage(string message)
    {
        this.message = message;
        dialogService!.Alert(message, "Success", new AlertOptions() { OkButtonText = "Ok" });
        StateHasChanged();
    }

    protected override void OnInitialized()
    {
        addHabitViewModel = new AddHabitViewModel(navigationManager!, apiService!, messageService!, iJSRuntime!);

        addHabitViewModel.AddHabitNameViewModel.FormSubmitRequested += OnAddHabitNameViewModelFormSubmitRequested;
        addHabitViewModel.AddHabitReasonViewModel.FormSubmitRequested += OnAddHabitReasonViewModelFormSubmitRequested;
        addHabitViewModel.AddHabitDaysViewModel.FormSubmitRequested += OnAddHabitDaysViewModelFormSubmitRequested;
        addHabitViewModel.AddHabitAlarmViewModel.FormSubmitRequested += OnAddHabitAlarmViewModelFormSubmitRequested;
        addHabitViewModel.AddHabitEarningsViewModel.FormSubmitRequested += OnAddHabitEarningsViewModelFormSubmitRequested;

        addHabitViewModel.PropertyChanged += (sender, args) =>
        {
            StateHasChanged();
        };

        messageService!.OnMessageReceived += HandleMessage;
    }
}
