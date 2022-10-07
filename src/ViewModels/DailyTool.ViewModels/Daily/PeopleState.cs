using CommunityToolkit.Mvvm.ComponentModel;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DailyTool.ViewModels.Daily
{
    public class PeopleState : ObservableObject, IPeopleState
    {
        private readonly IPersonService _personService;

        public PeopleState(IPersonService personService)
        {
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));

            People = new ObservableCollection<PersonViewModel>();
        }

        public ObservableCollection<PersonViewModel> People { get; }

        private async void OnPeopleChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var newPerson in GetPeopleFromChangedEvents(e.NewItems))
            {
                newPerson.PropertyChanged += OnPersonChanged;
                newPerson.Id = await _personService.CreatePersonAsync(new Person
                {
                    Name = newPerson.Name,
                    IsParticipating = newPerson.IsParticipating
                });
            }

            foreach (var oldPerson in GetPeopleFromChangedEvents(e.OldItems))
            {
                oldPerson.PropertyChanged -= OnPersonChanged;
                await _personService.DeletePersonAsync(oldPerson.Id);
            }
        }

        private IReadOnlyCollection<PersonViewModel> GetPeopleFromChangedEvents(IList? people)
        {
            if (people is null)
            {
                return Array.Empty<PersonViewModel>();
            }

            return people.OfType<PersonViewModel>().ToList();
        }

        private async void OnPersonChanged(object? sender, PropertyChangedEventArgs e)
        {
            var viewModel = sender as PersonViewModel;
            if (viewModel is null)
            {
                return;
            }

            await _personService.UpdatePersonAsync(new Person
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                IsParticipating = viewModel.IsParticipating
            });
        }

        public async Task LoadDataAsync()
        {
            var people = await _personService.GetAllAsync();
            foreach (var person in people)
            {
                People.Add(new PersonViewModel
                {
                    Id = person.Id,
                    Name = person.Name,
                    IsParticipating = person.IsParticipating
                });
            }

            People.CollectionChanged -= OnPeopleChanged;
            People.CollectionChanged += OnPeopleChanged;
        }
    }
}
