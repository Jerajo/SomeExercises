# Proyecto portable (Lógica de negocios)

La lógica de negocios, y las vistas se encuentran en este proyecto.

|  Título  | Descripción |
| ------------- | ------------- |
| Lenguaje | C# |
| Arquitectura  | [3-Tier](https://docs.microsoft.com/en-us/windows/win32/cossdk/using-a-three-tier-architecture-model) |
| Patrón de diseño  | [MVVM](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/mvvm)  |

**Vistas**
- [MainPage](Views/MainPage.xaml)
- [Exercise1](Views/Exercise1.xaml)
- [Exercise2](Views/Exercise2.xaml)
- [Exercise3](Views/Exercise3.xaml)
- [Exercise4](Views/Exercise4.xaml)

**Modelos**
- [FileModel](Models/FileModel.cs)
- [OperationModel](Models/OperationModel.cs)

**VistasModelos**
- [BaseViewModel](ViewsModels/BaseViewModel.cs)
- [Exercise1ViewModel](ViewsModels/Exercise1ViewModel.cs)
- [Exercise2ViewModel](Views/Exercise2ViewModel.cs)
- [Exercise3ViewModel](Views/Exercise3ViewModel.cs)
- [Exercise4ViewModel](Views/Exercise4ViewModel.cs)

Este proyecto utliza los siguientes NuGets

- Fody 3.3.5
- Newtonsoft.Json 12.0.2
- Xamarin.Essentials 1.2.0
- PropertyChanged.Fody 2.6.0
- Xamarin.Forms 4.1.0.581479
- Xamarin.Plugin.FilePicker 2.1.18
