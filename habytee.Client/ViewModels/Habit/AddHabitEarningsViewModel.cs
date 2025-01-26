namespace habytee.Client.ViewModels;

public class AddHabitEarningsViewModel : BaseViewModel
{
    private int earnings = 4;
    public int Earnings
    {
        get => earnings;
        set
        {
            earnings = value;
            if(earnings < 1)
            {
                earnings = 1;
            }
            Selected =
            [
                new() {
                    Position = earnings,
                    Value = CalculateFibonacci(earnings+1)
                }
            ];
            OnPropertyChanged(nameof(Selected));
            OnPropertyChanged();
        }
    }

    public class Fibonacci
    {
        public int Position { get; set; }
        public int Value { get; set; }
    }

    public Fibonacci[] Fibonaccis = new Fibonacci[] {
        new Fibonacci
        {
            Position = 0,
            Value = 0
        },
        new Fibonacci
        {
            Position = 1,
            Value = 1
        },
        new Fibonacci
        {
            Position = 2,
            Value = 2
        },
        new Fibonacci
        {
            Position = 3,
            Value = 3
        },
        new Fibonacci
        {
            Position = 4,
            Value = 5
        },
        new Fibonacci
        {
            Position = 5,
            Value = 8
        },
        new Fibonacci
        {
            Position = 6,
            Value = 13
        }
    };

    public Fibonacci[] Selected = new Fibonacci[] {
        new Fibonacci
        {
            Position = 4,
            Value = 5
        }
    };

    public AddHabitEarningsViewModel(BaseViewModel parentViewModels)
    {
        ParentViewModel = parentViewModels;
    }

    private static int CalculateFibonacci(int n)
    {
        if (n == 0) return 0;
        if (n == 1) return 1;

        int prev = 0, current = 1;

        for (int i = 2; i <= n; i++)
        {
            int next = prev + current;
            prev = current;
            current = next;
        }

        return current;
    }
}
