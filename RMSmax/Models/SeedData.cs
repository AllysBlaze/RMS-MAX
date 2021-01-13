using System.Linq;
using RMSmax.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace RMSmax.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            RMSContext context = app.ApplicationServices.GetRequiredService<RMSContext>();
            if (!context.Articles.Any())
            {
                context.Articles.AddRange(
                    new Article
                    {
                        Title = "Lorem ipsum",
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed vitae quam blandit, congue ante condimentum, sodales arcu. Sed egestas nibh a fermentum dictum. Donec vehicula, lacus sit amet consequat fringilla, diam dui mattis elit, ac pellentesque leo augue in nisi. Nullam iaculis cursus odio, eget aliquam nisi. Donec blandit mauris consequat, elementum urna vel, pharetra metus. Duis aliquam enim id ante ullamcorper, vel convallis risus efficitur. In lobortis maximus lectus. Curabitur vehicula magna sed odio ullamcorper dictum ut id libero. Etiam porttitor dolor ut nibh consequat gravida. Vivamus lacinia metus nec erat venenatis, varius pellentesque nisi volutpat.",
                        Author = "Author",
                        PhotoIn = "pic.jpg",
                        PhotoCover="pic4.jpg"
                    },
                    new Article
                    {
                        Title = "Pan Tadeusz",
                        Content = "Litwo! Ojczyzno moja! Ty jesteś jak zdrowie. Ile cię trzeba cenić, ten Bonapart czarował, no, tak wedle dzisiejszej mody odsyłać konie porzucone same szczypiąc trawę ciągnęły powoli pod las drogi i w pustki prowadzić. Po tem dawno w czasie krajowych urzędów przynajmniej tom skorzystał, że przychodził już pomrok mglisty napełniając wierzchołki i z chleba gałeczki trzy z powozu. konie porzucone same obicia z kim on zająca pochwycił. Asesor zaś Gotem. Dość, że odbite od ciemnej zieleni topoli, co dzień za kolana). On opowiadał, jako jenerał Dąbrowski z boku miał, w tem nic to mówiąc, że gotyckiej są łąki i każdego wodza legijonu i żądał. I starzy mówili myśliwi młodzi tak myślili starzy. A na końcu do swawoli. Z wieku mu słowo ciocia koło uch brzęczało ciągle Sędziemu tłumaczył dlaczego urządzenie pańskie jachał szlachcic młody panek i Moskalom przez okienic szpar i dalszych replik stronom dzisiaj nie było gorąca). wachlarz dla Rosyi straszną jak światłość miesiąca. Nucąc chwyciła suknie, biegła do Litwy kwestarz z boku miał, w domu ziemię orzę gdy potem cały las drogi i poprawiwszy nieco poróżnieni bo tak.",
                        Author = "Adam Mickiewicz",
                        PhotoCover="pic.jpg"
                    },
                    new Article
                    {
                        Title = "Rozprawa",
                        Content = "Początek traktatu czasu panowania Fryderyka Wielkiego, Króla Pruskiego żył w naszej poprawy przestępcy, a kto oczekuje swojej natury naszej mocy i praktycznie konieczne; bo tu przyjdzie wyznać, że to co my z powszechnie płatnym lub złego losu sobie przedstawiamy sobie Dobra poznajemy. . Pojęcie o najwyższej Istności, ale kiedy ja jej pierwiastkowość i dobrego jest nagrodą wstrzemięźliwości lub czynnym, lecz nie jeden drugiemu wspólnie dopomagali. A gdyby więc sobie pomyśleć można. Co? czy chcemy być niedostatecznemi, to jest więc też i niezawisłe od Stworzyciela niebył zachwalił i uszczęśliwił. Może on jest osobna zła rzecz właśnie z owym mniemanym szczęściem. Robota, trudności, praca, fatyga, oczekiwany odpoczynek, usiłowanie dojść do tego, który mamy w jego majątek, ale świat są wyraźnie oznaczone, a zatym i przesypiają, się. Żaden człowiek, aby się sprawiedliwości Dobraj. Kiedy się komukolwiek źle powodziło, lecz on go znowu zło. Więc zło samo przez rozum swoj wykształcał. Tak może np. będzie w sobie zawiera w niemieckim rękopiśmie. Te ostatnie zostawił tłumacz, bo tu właśnie to ograniczenie dobroci przez się dalej postępuje i że raczej dla obiecanego pożytku pracuje, tego czynność.",
                        Author = "Immanuel Kant",
                        PhotoIn = "pic3.jpg",
                        PhotoCover = "pic5.jpg"
                    },
                    new Article
                    {
                        Title = "Tytuł Tekstu",
                        Content = "Krótki tekst przykładowy",
                        Author = "Author",
                        PhotoIn = "pic4.jpg",
                    },
                    new Article
                    {
                        Title = "Tytuł Tekstu",
                        Content = "Krótki tekst przykładowy",
                        Author = "Author",
                        PhotoIn = "pic6.jpg",
                        PhotoCover = "pic2.jpg"
                    },
                    new Article
                    {
                        Title = "Pan Tadeusz",
                        Content = "Litwo! Ojczyzno moja! Ty jesteś jak zdrowie. Ile cię trzeba cenić, ten Bonapart czarował, no, tak wedle dzisiejszej mody odsyłać konie porzucone same szczypiąc trawę ciągnęły powoli pod las drogi i w pustki prowadzić. Po tem dawno w czasie krajowych urzędów przynajmniej tom skorzystał, że przychodził już pomrok mglisty napełniając wierzchołki i z chleba gałeczki trzy z powozu. konie porzucone same obicia z kim on zająca pochwycił. Asesor zaś Gotem. Dość, że odbite od ciemnej zieleni topoli, co dzień za kolana). On opowiadał, jako jenerał Dąbrowski z boku miał, w tem nic to mówiąc, że gotyckiej są łąki i każdego wodza legijonu i żądał. I starzy mówili myśliwi młodzi tak myślili starzy. A na końcu do swawoli. Z wieku mu słowo ciocia koło uch brzęczało ciągle Sędziemu tłumaczył dlaczego urządzenie pańskie jachał szlachcic młody panek i Moskalom przez okienic szpar i dalszych replik stronom dzisiaj nie było gorąca). wachlarz dla Rosyi straszną jak światłość miesiąca. Nucąc chwyciła suknie, biegła do Litwy kwestarz z boku miał, w domu ziemię orzę gdy potem cały las drogi i poprawiwszy nieco poróżnieni bo tak.",
                        Author = "Adam Mickiewicz",
                        PhotoIn = "pic2.jpg",
                        PhotoCover = "pic2.jpg",
                    },
                    new Article
                    {
                        Title = "Tytuł Tekstu",
                        Content = "Krótki tekst przykładowy",
                        Author = "Author",
                        PhotoIn = "pic4.jpg",
                        PhotoCover = "pic4.jpg",
                    },
                    new Article
                    {
                        Title = "Tytuł Tekstu",
                        Content = "Krótki tekst przykładowy",
                        Author = "Author",
                        PhotoIn = "pic.jpg",
                        PhotoCover = "pic.jpg",
                    },
                    new Article
                    {
                        Title = "Lorem ipsum",
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed vitae quam blandit, congue ante condimentum, sodales arcu. Sed egestas nibh a fermentum dictum. Donec vehicula, lacus sit amet consequat fringilla, diam dui mattis elit, ac pellentesque leo augue in nisi. Nullam iaculis cursus odio, eget aliquam nisi. Donec blandit mauris consequat, elementum urna vel, pharetra metus. Duis aliquam enim id ante ullamcorper, vel convallis risus efficitur. In lobortis maximus lectus. Curabitur vehicula magna sed odio ullamcorper dictum ut id libero. Etiam porttitor dolor ut nibh consequat gravida. Vivamus lacinia metus nec erat venenatis, varius pellentesque nisi volutpat.",
                        Author = "Author",
                    },
                    new Article
                    {
                        Title = "Lorem Ipsum",
                        Content = "Godfather ipsum dolor sit amet. We're both part of the same hypocrisy, senator, but never think it applies to my family. The hotel, the casino. The Corleone Family wants to buy you out. Don't you know that I would use all of my power to prevent something like that from happening? My father is no different than any powerful man, any man with power, like a president or senator",
                        Author = "Mario Puzo",
                        PhotoIn = "pic.jpg",
                        PhotoCover = "pic.jpg",
                    },
                    new Article
                    {
                        Title = "Pan Tadeusz",
                        Content = "Litwo! Ojczyzno moja! Ty jesteś jak zdrowie. Ile cię trzeba cenić, ten Bonapart czarował, no, tak wedle dzisiejszej mody odsyłać konie porzucone same szczypiąc trawę ciągnęły powoli pod las drogi i w pustki prowadzić. Po tem dawno w czasie krajowych urzędów przynajmniej tom skorzystał, że przychodził już pomrok mglisty napełniając wierzchołki i z chleba gałeczki trzy z powozu. konie porzucone same obicia z kim on zająca pochwycił. Asesor zaś Gotem. Dość, że odbite od ciemnej zieleni topoli, co dzień za kolana). On opowiadał, jako jenerał Dąbrowski z boku miał, w tem nic to mówiąc, że gotyckiej są łąki i każdego wodza legijonu i żądał. I starzy mówili myśliwi młodzi tak myślili starzy. A na końcu do swawoli. Z wieku mu słowo ciocia koło uch brzęczało ciągle Sędziemu tłumaczył dlaczego urządzenie pańskie jachał szlachcic młody panek i Moskalom przez okienic szpar i dalszych replik stronom dzisiaj nie było gorąca). wachlarz dla Rosyi straszną jak światłość miesiąca. Nucąc chwyciła suknie, biegła do Litwy kwestarz z boku miał, w domu ziemię orzę gdy potem cały las drogi i poprawiwszy nieco poróżnieni bo tak.",
                        Author = "Adam Mickiewicz",
                        PhotoIn = "pic2.jpg",
                        PhotoCover = "pic5.jpg",
                    },
                    new Article
                    {
                        Title = "Tytuł Tekstu",
                        Content = "Krótki tekst przykładowy",
                        Author = "Author",
                        PhotoIn = "pic6.jpg",
                        PhotoCover = "pic6.jpg",
                    },
                    new Article
                    {
                        Title = "Duma i uprzedzenie",
                        Content = "Mr. Bennet saw that her whole heart was in the subject, and affectionately taking her hand said in reply: “Do not make yourself uneasy my love.Wherever you and Jane are known you must be respected and valued; and you will not appear to less advantage for having a couple of--or I may say, three--very silly sisters.We shall have no peace at Longbourn if Lydia does not go to Brighton.Let her go, then.Colonel Forster is a sensible man, and will keep her out of any real mischief; and she is luckily too poor to be an object of prey to anybody.At Brighton she will be of less importance even as a common flirt than she has been here.The officers will find women better worth their notice.Let us hope, therefore, that her being there may teach her her own insignificance. At any rate, she cannot grow many degrees worse, without authorising us to lock her up for the rest of her life.”",
                        Author = "Jane Austen",
                        PhotoIn = "pic3.jpg",
                        PhotoCover = "pic4.jpg",
                    },
                    new Article
                    {
                        Title = "Lorem ipsum",
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed vitae quam blandit, congue ante condimentum, sodales arcu. Sed egestas nibh a fermentum dictum. Donec vehicula, lacus sit amet consequat fringilla, diam dui mattis elit, ac pellentesque leo augue in nisi. Nullam iaculis cursus odio, eget aliquam nisi. Donec blandit mauris consequat, elementum urna vel, pharetra metus. Duis aliquam enim id ante ullamcorper, vel convallis risus efficitur. In lobortis maximus lectus. Curabitur vehicula magna sed odio ullamcorper dictum ut id libero. Etiam porttitor dolor ut nibh consequat gravida. Vivamus lacinia metus nec erat venenatis, varius pellentesque nisi volutpat.",
                        Author = "Author",
                        PhotoIn = "pic4.jpg",
                        PhotoCover = "pic4.jpg",
                    },
                    new Article
                    {
                        Title = "Tytuł Tekstu",
                        Content = "Krótki tekst przykładowy",
                        Author = "Author",
                        PhotoCover = "pic2.jpg",
                        PhotoIn = "pic6.jpg",
                    },
                    new Article
                    {
                        Title = "Pan Tadeusz",
                        Content = "Litwo! Ojczyzno moja! Ty jesteś jak zdrowie. Ile cię trzeba cenić, ten Bonapart czarował, no, tak wedle dzisiejszej mody odsyłać konie porzucone same szczypiąc trawę ciągnęły powoli pod las drogi i w pustki prowadzić. Po tem dawno w czasie krajowych urzędów przynajmniej tom skorzystał, że przychodził już pomrok mglisty napełniając wierzchołki i z chleba gałeczki trzy z powozu. konie porzucone same obicia z kim on zająca pochwycił. Asesor zaś Gotem. Dość, że odbite od ciemnej zieleni topoli, co dzień za kolana). On opowiadał, jako jenerał Dąbrowski z boku miał, w tem nic to mówiąc, że gotyckiej są łąki i każdego wodza legijonu i żądał. I starzy mówili myśliwi młodzi tak myślili starzy. A na końcu do swawoli. Z wieku mu słowo ciocia koło uch brzęczało ciągle Sędziemu tłumaczył dlaczego urządzenie pańskie jachał szlachcic młody panek i Moskalom przez okienic szpar i dalszych replik stronom dzisiaj nie było gorąca). wachlarz dla Rosyi straszną jak światłość miesiąca. Nucąc chwyciła suknie, biegła do Litwy kwestarz z boku miał, w domu ziemię orzę gdy potem cały las drogi i poprawiwszy nieco poróżnieni bo tak.",
                        Author = "Adam Mickiewicz",
                        PhotoIn = "pic4.jpg",
                        PhotoCover = "pic4.jpg",
                    },
                    new Article
                    {
                        Title = "Lorem ipsum",
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed vitae quam blandit, congue ante condimentum, sodales arcu. Sed egestas nibh a fermentum dictum. Donec vehicula, lacus sit amet consequat fringilla, diam dui mattis elit, ac pellentesque leo augue in nisi. Nullam iaculis cursus odio, eget aliquam nisi. Donec blandit mauris consequat, elementum urna vel, pharetra metus. Duis aliquam enim id ante ullamcorper, vel convallis risus efficitur. In lobortis maximus lectus. Curabitur vehicula magna sed odio ullamcorper dictum ut id libero. Etiam porttitor dolor ut nibh consequat gravida. Vivamus lacinia metus nec erat venenatis, varius pellentesque nisi volutpat.",
                        Author = "Author",
                        PhotoIn = "pic.jpg",
                        PhotoCover = "pic.jpg",
                    },
                    new Article
                    {
                        Title = "Rozprawa",
                        Content = "Początek traktatu czasu panowania Fryderyka Wielkiego, Króla Pruskiego żył w naszej poprawy przestępcy, a kto oczekuje swojej natury naszej mocy i praktycznie konieczne; bo tu przyjdzie wyznać, że to co my z powszechnie płatnym lub złego losu sobie przedstawiamy sobie Dobra poznajemy. . Pojęcie o najwyższej Istności, ale kiedy ja jej pierwiastkowość i dobrego jest nagrodą wstrzemięźliwości lub czynnym, lecz nie jeden drugiemu wspólnie dopomagali. A gdyby więc sobie pomyśleć można. Co? czy chcemy być niedostatecznemi, to jest więc też i niezawisłe od Stworzyciela niebył zachwalił i uszczęśliwił. Może on jest osobna zła rzecz właśnie z owym mniemanym szczęściem. Robota, trudności, praca, fatyga, oczekiwany odpoczynek, usiłowanie dojść do tego, który mamy w jego majątek, ale świat są wyraźnie oznaczone, a zatym i przesypiają, się. Żaden człowiek, aby się sprawiedliwości Dobraj. Kiedy się komukolwiek źle powodziło, lecz on go znowu zło. Więc zło samo przez rozum swoj wykształcał. Tak może np. będzie w sobie zawiera w niemieckim rękopiśmie. Te ostatnie zostawił tłumacz, bo tu właśnie to ograniczenie dobroci przez się dalej postępuje i że raczej dla obiecanego pożytku pracuje, tego czynność.",
                        Author = "Immanuel Kant",
                        PhotoIn = "pic6.jpg",
                    }
                    );



            }
            if (!context.Employees.Any())
            {
                context.Employees.AddRange(
                    new Employee
                    {
                        Name = "Marcela",
                        LastName = "Zielińska",
                        Degree = "prof. dr hab. inż",
                        Mail = "Marcela.Zielinska@uczelnia.pl",
                        Phone = "887987098",
                        Photo = "woman1.jpg",

                    },
                    new Employee
                    {
                        Name = "Pamela",
                        LastName = "Jakubowska",
                        Degree = "lic.",
                        Mail = "Pamela.Jakubowska@uczelnia.pl",
                        Phone = "888777666",
                        Photo = "woman2.jpg",

                    },
                    new Employee
                    {
                        Name = "Dobromił",
                        LastName = "Mazur",
                        Degree = "Prof. dr hab.",
                        Mail = "Dobromil.Mazur@uczelnia.pl",
                        Department = "Katedra Matematyki",

                    },
                    new Employee
                    {
                        Name = "Alex",
                        LastName = "Duda",
                        Degree = "inż.",
                        Mail = "Alex.Duda@uczelnia.pl",
                        Phone = "000000000",
                        Photo = "man2.jpg",

                    },
                    new Employee
                    {
                        Name = "Idalia",
                        LastName = "Nowak",
                        Degree = "prof.",
                        Mail = "Idalia.Nowak@uczelnia.pl",
                        Photo = "woman1.jpg",
                        Department = "Katedra Informatyki",
                        Function = "Prodziekan",
                        Room = "MS 505",
                        Timetable = "https://plan.polsl.pl/plan.php?type=10&id=189403&winW=740&winH=674&loadBG=000000",

                    }, new Employee
                    {
                        Name = "Jowita",
                        LastName = "Krajewska",
                        Degree = "mgr.",
                        Mail = "Jowita.Krajewska@uczelnia.pl",
                        Department = "Katedra Informatyki",
                        Position = "Asystent",
                        Room = "MS 505",
                        Timetable = "https://plan.polsl.pl/plan.php?type=10&id=141",

                    }, new Employee
                    {
                        Name = "Milan",
                        LastName = "Zawadzki",
                        Degree = "dr hab. inż.",
                        Mail = "Milan.Zawadzki@uczelnia.pl",
                        Phone = "88664422",
                        Photo = "man2.jpg",
                        Department = "Katedra Informatyki",
                        Function = "Prodziekan",
                        Room = "MS 505",
                        Timetable = "https://plan.polsl.pl/plan.php?type=10&id=4685&winW=740&winH=674&loadBG=000000",

                    }, new Employee
                    {
                        Name = "Norbert",
                        LastName = "Lis",
                        Degree = "mgr. inż",
                        Mail = "Norbert.Lis@uczelnia.pl",
                        Photo = "man1.jpg",
                        Department = "Katedra Informatyki",
                        Position = "Asystent",
                        Room = "MS 505",
                        Timetable = "https://plan.polsl.pl/plan.php?type=10&id=4685&winW=740&winH=674&loadBG=000000",

                    }, new Employee
                    {
                        Name = "Oktawia",
                        LastName = "Wójcik",
                        Degree = "dr hab.",
                        Mail = "Oktawia.Wojcik@uczelnia.pl",
                        Photo = "woman2.jpg",
                        Department = "Katedra Informatyki",
                        Room = "MS 505",
                        Timetable = "https://plan.polsl.pl/plan.php?type=10&id=189403&winW=740&winH=674&loadBG=000000",

                    }, new Employee
                    {
                        Name = "Ada",
                        LastName = "Michalska",
                        Degree = "mgr.",
                        Mail = "Ada.Michalska@uczelnia.pl",
                        Photo = "woman1.jpg",
                        Department = "Katedra Matematyki",
                        Room = "MS 505",
                        Timetable = "https://plan.polsl.pl/plan.php?type=10&id=141",

                    }, new Employee
                    {
                        Name = "Alisa",
                        LastName = "Sikorska",
                        Degree = "dr",
                        Mail = "Alisa.Sikorska@uczelnia.pl",
                        Department = "Katedra Informatyki",
                        Room = "MS 505",
                        Timetable = "https://plan.polsl.pl/plan.php?type=10&id=4685&winW=740&winH=674&loadBG=000000",

                    }, new Employee
                    {
                        Name = "Józej",
                        LastName = "Tomaszewski",
                        Degree = "dr inż.",
                        Mail = "Jozef.Tomaszewski@uczelnia.pl",
                        Department = "Katedra Matematyki",
                        Room = "MS 505",
                        Timetable = "https://plan.polsl.pl/plan.php?type=10&id=141",

                    }, new Employee
                    {
                        Name = "Jerzy",
                        LastName = "Sobczak",
                        Degree = "mgr inż.",
                        Mail = "Jerzy.Sobczak@uczelnia.pl",
                        Photo = "man2.jpg",
                        Department = "Katedra Informatyki",
                        Room = "MS 505",
                        Timetable = "https://plan.polsl.pl/plan.php?type=10&id=189403&winW=740&winH=674&loadBG=000000",

                    },
                    new Employee
                    {
                        Name = "Bogumił",
                        LastName = "Kalinowski",
                        Degree = "lic.",
                        Mail = "Bogumil.Kalinowski@uczelnia.pl",
                        Department = "Katedra Matematyki",
                        Room = "MS 505",
                        Timetable = "https://plan.polsl.pl/plan.php?type=10&id=4685&winW=740&winH=674&loadBG=000000",

                    }, new Employee
                    {
                        Name = "Rafał",
                        LastName = "Zawadzki",
                        Degree = "dr",
                        Mail = "Rafal.Zawadzki@uczelnia.pl",
                        Photo = "man1.jpg",
                        Department = "Katedra Informatyki",
                        Timetable = "https://plan.polsl.pl/plan.php?type=10&id=141",

                    }, new Employee
                    {
                        Name = "Matylda",
                        LastName = "Czerwińska",
                        Degree = "prof",
                        Mail = "Matylda.Czerwinska@uczelnia.pl",
                        Photo = "woman2.jpg",
                        Department = "Katedra Matematyki",
                        Timetable = "https://plan.polsl.pl/plan.php?type=10&id=4685&winW=740&winH=674&loadBG=000000",

                    }
                    );
            }
            if (!context.Subjects.Any())
            {
                context.Subjects.AddRange(
                    new Subject
                    {
                        Course = "Informatyka",
                        Degree = 2,
                        File = "inf_II_2_20_Grafy_i_sieci.pdf",
                        Name = "Teoria grafów i sieci",
                        Semester = 2,
                    },
                    new Subject
                    {
                        Course = "Informatyka",
                        Degree = 1,
                        File = "inf_I_2_19_Laboratorium_techniki_komputerowej.pdf",
                        Name = "Labolatorium techniki komputerowej",
                        Semester = 2,
                    },
                    new Subject
                    {
                        Course = "Informatyka",
                        Degree = 1,
                        File = "inf_I_3_18_Jezyki_skryptowe.pdf",
                        Name = "Języki skryptowe",
                        Semester = 3,
                    },
                    new Subject
                    {
                        Course = "Informatyka",
                        Degree = 1,
                        File = "inf_I_4_18_Bazy_danych.pdf",
                        Name = "Bazy danych",
                        Semester = 4,
                    },
                    new Subject
                    {
                        Course = "Informatyka",
                        Degree = 1,
                        File = "inf_I_3_18_Jezyki_skryptowe.pdf",
                        Name = "Inżynieria oprogramowania",
                        Semester = 5,
                    },
                    new Subject
                    {
                        Course = "Informatyka",
                        Degree = 2,
                        File = "inf_I_6_17_PO4_Fotografia_cyfrowa.pdf",
                        Name = "Fotografia cyfrowa",
                        Semester = 1,
                    },
                    new Subject
                    {
                        Course = "Informatyka",
                        Degree = 2,
                        File = "inf_II_1_20_Matematyka_stosowana.pdf",
                        Name = "Matematyka stosowana",
                        Semester = 1,
                    },
                    new Subject
                    {
                        Course = "Matematyka",
                        Degree = 1,
                        File = "mat_I_1_20_Technologia_informacyjna.pdf",
                        Name = "Technologia Informatyczna",
                        Semester = 1,
                    },
                    new Subject
                    {
                        Course = "Matematyka",
                        Degree = 1,
                        File = "mat_I_2_20_Analiza_matematyczna_II.pdf",
                        Name = "Analiza Matematyczna II",
                        Semester = 2,
                    },
                    new Subject
                    {
                        Course = "Matematyka",
                        Degree = 1,
                        File = "mat_I_4_19_Discrete_mathematics.pdf",
                        Name = "Algorytmy i struktury danych",
                        Semester = 3,
                    }, new Subject
                    {
                        Course = "Matematyka",
                        Degree = 1,
                        File = "mat_I_4_19_Discrete_mathematics.pdf",
                        Name = "Discrete Mathematics",
                        Semester = 4,
                    }, new Subject
                    {
                        Course = "Matematyka",
                        Degree = 2,
                        File = "mat_II_1_20_Modelowanie i symulacja stochastyczna.pdf",
                        Name = "Modelowanie i symulacja stochastyczna",
                        Semester = 1,
                    }, new Subject
                    {
                        Course = "Matematyka",
                        Degree = 2,
                        File = "mat_II_2_20_Programowanie_obiektowe.pdf",
                        Name = "Programowanie obiektowe",
                        Semester = 2,
                    }
                    );
            }
            if (!context.StudentsTimetables.Any())
            {
                context.StudentsTimetables.AddRange(
                    new StudentsTimetable
                    {
                        Course = "Matematyka",
                        Degree = 1,
                        Semester = 1,
                        Timetable = "https://plan.polsl.pl/plan.php?type=2&id=12682"
                    },

                    new StudentsTimetable
                    {
                        Course = "Matematyka",
                        Degree = 1,
                        Semester = 4,
                        Timetable = "https://plan.polsl.pl/plan.php?type=2&id=4795"
                    },
                    new StudentsTimetable
                    {
                        Course = "Matematyka",
                        Degree = 2,
                        Semester = 2,
                        Timetable = "https://plan.polsl.pl/plan.php?type=2&id=5394"
                    },
                    new StudentsTimetable
                    {
                        Course = "Matematyka",
                        Degree = 2,
                        Semester = 3,
                        Timetable = "https://plan.polsl.pl/plan.php?type=2&id=12694"
                    },
                    new StudentsTimetable
                    {
                        Course = "Informatyka",
                        Degree = 1,
                        Semester = 1,
                        Timetable = "https://plan.polsl.pl/plan.php?type=2&id=13169"
                    },
                    new StudentsTimetable
                    {
                        Course = "Informatyka",
                        Degree = 1,
                        Semester = 2,
                        Timetable = "https://plan.polsl.pl/plan.php?type=2&id=15488"

                    }, new StudentsTimetable
                    {
                        Course = "Informatyka",
                        Degree = 1,
                        Semester = 3,
                        Timetable = "https://plan.polsl.pl/plan.php?type=2&id=26519"

                    }, new StudentsTimetable
                    {
                        Course = "Informatyka",
                        Degree = 1,
                        Semester = 4,
                        Timetable = "https://plan.polsl.pl/plan.php?type=2&id=51427"

                    },
                     new StudentsTimetable
                     {
                         Course = "Informatyka",
                         Degree = 1,
                         Semester = 5,
                         Timetable = "https://plan.polsl.pl/plan.php?type=2&id=73795"

                     },
                    new StudentsTimetable
                    {
                        Course = "Informatyka",
                        Degree = 2,
                        Semester = 1,
                        Timetable = "https://plan.polsl.pl/plan.php?type=2&id=343240280"
                    },
                    new StudentsTimetable
                    {
                        Course = "Informatyka",
                        Degree = 2,
                        Semester = 2,
                        Timetable = "https://plan.polsl.pl/plan.php?type=2&id=343257361"

                    }

                    );
            }
            context.SaveChanges();
        }
    }
}
