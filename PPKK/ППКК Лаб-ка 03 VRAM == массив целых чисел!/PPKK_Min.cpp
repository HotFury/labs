// Курс: Проектирование программных компонент и комплексов (ППКК)
// Лектор: доц. Евгений Александрович Лобода
//"Вывод окна, управляемого методами DirectDraw библиотеки DirectX v.7" 
/***************************************************************************/
// ЗАДАНИЕ: по нажатию на клавиатуре клавиши 'F1' : 
// 1 - вывести параметры текущего режима монитора (Разрешение по X,Разрешение по Y,Бит в пикселе,Частоту кадров).
// 2 - выполнить прямое изменение пикселей рабочей области экрана (нарисовать линии разных цветов).
/***************************************************************************/
// НИЖЕ: Минимальная API-программа проекта для Win98/NT/2000/XP/Win2003,          
// состоящая из 2-х функций: оконной функции WndProc() и главной функции WinMain() 
/***************************************************************************/
#define WIN32_LEAN_AND_MEAN // для ускорения запуска процесса
#include <windows.h>        // для доступа к API функциям Windows
#include <stdio.h>          // для доступа к sprintf()
#include <ddraw.h>          // для доступа к DirectX
#include <mmsystem.h>       // для доступа к Multimedia API's DirectX

// Глобальные объявления для использования во всех функциях проекта
char Str[200],*pStr=NULL;   // для формирования текстовых строк
HWND hWndG;   // для использования значения hWnd в разных функциях проекта

/****************************************************************************/
// ОКОННАЯ ФУНКЦИЯ == действия программы по обработке сообщений в Windows 
LRESULT CALLBACK WndProc(HWND hWnd,UINT uMsgName,WPARAM wParam,LPARAM lParam)
{static LPDIRECTDRAW7        pDirectDraw7=NULL;   // для pointer-а на класс DirectDraw
 static LPDIRECTDRAWSURFACE7 pPrimarySurface=NULL;// для pointer-а на первичную поверхность (Surface)
        HRESULT hRet; //Переменная для возвращаемых функциями кодов ошибок
                      //(использовать не будем, НО НАДО!)
 static HDC hDC;      //для GDI контекста вывода на экран
 static int nDDraw=0; //счетчик подсчета числа запущенных экземпляров класса DirectDraw
 static int nF1=0;//счетчик подсчета числа нажатий клавиши 'F1'
 
/*************************************************************************/
 switch(uMsgName) // Обработка сообщения:
/*************************************************************************/
  {case WM_KEYDOWN://при нажатии любой клавиши(по WM_CREATE лучше, т.к. до появления изображения окна)
    if(wParam==VK_F1)//если нажата клавиша 'F1'
    {  nF1++;//число нажатий клавиши 'F1'
     //Создание экземпляра объекта DirectDraw библиотеки виртуальных классов DirectX v.7  
     //Получаем pointer на DirectDraw указанием его интерфейса в DirectX v.7.0
        pStr="DirectDraw7 не обновлялся по этому нажатию F1 ";//для контроля обновления DirectDraw
      //if(nDDraw==0)//??? делаем только один экземпляр объекта DirectDraw при всех нажатиях 'F1' ???
        if((nDDraw==0)|(nDDraw==1))//!!! делаем экземпляр объекта DirectDraw дважды при всех нажатиях 'F1' ???
	    {//обнуление интерфейса перед обновлением значения
           pDirectDraw7=NULL;
		 //создание экземпляра (интерфейса) DirectDraw 
		 hRet=DirectDrawCreateEx(NULL,// == текущий гр.адаптер (можно отключить аксерераторы или их эмуляцию)
                (VOID**)&pDirectDraw7,// получаем pointer входа в DirectDraw7 (доступ к его методам)
                     IID_IDirectDraw7,// имя интерфейса DirectDraw7
                                NULL);// не используется
         //Контроль создания экземпляра объекта DirectDraw  
           if(hRet==DD_OK)pStr="СОЗДАН новый экземпляр объекта DirectDraw7";
           if(hRet!=DD_OK)pStr="Error при создании DirectDraw7";
           if(hRet==DDERR_DIRECTDRAWALREADYCREATED)pStr="DirectDraw7 уже был создан";
           nDDraw++;//класс DirectDraw создан
          }
      
       // Задание уровня использования видеоадаптера (есть 12 уровней)
           hRet=pDirectDraw7->SetCooperativeLevel(hWnd,
           DDSCL_EXCLUSIVE | DDSCL_FULLSCREEN);//монопольно, весь экран
       //  DDSCL_NORMAL);// == Windows окно (работает при стиле WS_OVERLAPPEDWINDOW в CreateWindowEx()
	   // ВНИМАНИЕ:
	   // Изменения уровня использования видеоадаптера функцией SetCooperativeLevel() 
	   // требует при повторном входе в оконную функцию обновить экземпляр класса DirectDraw.

// /* 
	  //Задание графического режима вывода на экран 
	   hRet=pDirectDraw7->SetDisplayMode(1920, // пикселей по x
	                                     1080, // пикселей по y
	                                      32, // бит на пиксель
	                                  //  16, // бит на пиксель
	                                  //   8, // бит на пиксель
                                        NULL, // частота кадров прежняя
                                       NULL);// не используется
	   //ВНИМАНИЕ:
	   // Выполнение SetDisplayMode()прекращает (досрочно)этот вызов оконной функции
	   // (не выполняются операторы ниже строки с SetDisplayMode()).
	   // Поэтому, для выполнения пропущенных операторов (ниже SetDisplayMode())
	   // надо вторично нажать клавишу F1 (== опять войти в оконную функцию)
	   // и обновить класс DirectDraw (как и после SetCooperativeLevel()). 
       
// */

//==================================
//!!!СОЗДАНИЕ ПОВЕРХНОСТИ, подконтрольной средствам DurectX v.7:
    DDSURFACEDESC2 ddSurfaceDesc={NULL};//структура для параметров поверхности 
                                        //c обнулением ее полей !!!
  //ZeroMemory(&ddSurfaceDesc, sizeof(ddSurfaceDesc));//обнуление из "Учебников"

	//Установка необходимых начальных значений полей структуры DDSURFACEDESC2
	ddSurfaceDesc.dwSize=sizeof(ddSurfaceDesc); // поле размера структуры 
        ddSurfaceDesc.dwFlags = DDSD_CAPS;
        ddSurfaceDesc.ddsCaps.dwCaps = DDSCAPS_PRIMARYSURFACE;//разрешаем только ПЕРВИЧНУЮ поверхность
	ddSurfaceDesc.dwBackBufferCount=0;//вторичные поверхности BackBuffer не нужны

	//Создание ПЕРВИЧНОЙ поверхности (видимой на экране)
	hRet=pDirectDraw7->CreateSurface(&ddSurfaceDesc,//структура параметров поверхности   
                           &pPrimarySurface,//получаем pointer на поверхность (адрес ее начала в VRAM)
                           NULL);

//==================================
//ВЫВОД на ПЕРВИЧНУЮ ПОВЕРХНОСТЬ (рабочую область окна)	
//----------------------------------
//1 ВЫВОД (средствами GDI) ТЕКСТА с параметрами текущего режима
// /*
	//Определение параметров текущего режима (== поверхности) 
    hRet=pDirectDraw7->GetDisplayMode(&ddSurfaceDesc);

    //Блокируем ПЕРВИЧНУЮ поверхность (== рабочую область экрана)
	hRet=pPrimarySurface->Lock(NULL,&ddSurfaceDesc, 
	     DDLOCK_SURFACEMEMORYPTR | DDLOCK_WAIT, NULL);
     
    //Получение GDI контекста для вывода
	hRet=pPrimarySurface->GetDC(&hDC);
          
	//ВЫВОД текста средствами GDI (выводим параметры текущего режима) 	sprintf(Str,"ВЫВОД в DirectDraw-окно (см. ниже):");
          sprintf(Str,"ВЫВОД в DirectDraw-окно (см. ниже):");
          TextOut(hDC,10,130,Str,strlen(Str));//вывод текста
	  sprintf(Str,"nF1=%d",nF1);
	  TextOut(hDC,400,130,Str,strlen(Str));//номер нажатия на F1 

          TextOut(hDC,10,160,pStr,strlen(pStr));//вывод текста о экземпляре DirectDraw
	//контроль изменения адреса расположения объекта DirectDraw7:
	  sprintf(Str,"pDirectDraw7=%p  &ddSurfaceDesc=%p",pDirectDraw7,&ddSurfaceDesc);
	  TextOut(hDC,400,160,Str,strlen(Str));//вывод текста об обновлении адреса экземпляра DirectDraw

	  sprintf(Str,"Width=%i  Height=%u  PixelFormat=%d  RefreshRate=%d ",
	  ddSurfaceDesc.dwWidth,ddSurfaceDesc.dwHeight,
	  ddSurfaceDesc.ddpfPixelFormat.dwRGBBitCount,ddSurfaceDesc.dwRefreshRate);
	  TextOut(hDC,10,190,Str,strlen(Str));//вывод текста о текущем режиме
	  
	//контроль изменения адресов поверхностей (Surface):
	  sprintf(Str,"pPrimarySurface=%p  &pPrimarySurface=%p ",pPrimarySurface,&pPrimarySurface);
	  TextOut(hDC,400,190,Str,strlen(Str));//вывод текста об обновлении &ddSurfaceDesc и &pPrimarySurface


	pPrimarySurface->ReleaseDC(hDC);//удаляем GDI контекст

	pPrimarySurface->Unlock(NULL);//прекращаем блокирование доступа к ПЕРВИЧНОЙ поверхности

// */

//----------------------------------
//2 ПРЯМОЙ ДОСТУП к пикселям поверхности (ПОВЕРХНОСТЬ == МАССИВ целых чисел !!!)
//  Рисуем линии разных цветов: 

// /*

  //Определение параметров текущего режима (первичной поверхности) 
    hRet=pDirectDraw7->GetDisplayMode(&ddSurfaceDesc);
    //Блокируем ПЕРВИЧНУЮ поверхность (рабочую область экрана)
	hRet=pPrimarySurface->Lock(NULL,&ddSurfaceDesc,
                              DDLOCK_SURFACEMEMORYPTR | DDLOCK_WAIT, NULL);
    
    //Объявление pointer-а == МАССИВА БАЙТОВ с присвоением адреса начала поверхности
	//(в нем шаг движения по адресам == 1байт)
	UCHAR *PrimerBuf=(UCHAR *)ddSurfaceDesc.lpSurface;
	                                                  
	//Основные параметры для перемещения по поверхности 
      int baitInStrokePoverhn=ddSurfaceDesc.lPitch;//байт в строке
      int baitInPixel=ddSurfaceDesc.ddpfPixelFormat.dwRGBBitCount/8;//байт в пикселе

	//Рисуем линии разных цветов (!!! DirectDraw ИСКАЖАЕТ работу макроса RGB() !!!) 
	int x=10,y=220;//(!!! ПРОВЕРИТЬ ЦВЕТА линий и при разных форматах пикселей, задаваемых в SetDisplayMode() )
    for(int i=0;i<300;i++)
     {*((DWORD*)(PrimerBuf+(x+i)*baitInPixel+(y+00)*baitInStrokePoverhn))=
                     RGB(rand()%256,rand()%256,rand()%256); //случайный цвет
      *((DWORD*)(PrimerBuf+(x+i)*baitInPixel+(y+10)*baitInStrokePoverhn))=
                     RGB(rand()%256,rand()%256,rand()%256); //случайный цвет
      *((DWORD*)(PrimerBuf+(x+i)*baitInPixel+(y+20)*baitInStrokePoverhn))=RGB(85, 0, 0);//  белые пиксели   
      *((DWORD*)(PrimerBuf+(x+i)*baitInPixel+(y+30)*baitInStrokePoverhn))=0x00FFFFFF;//белые   пиксели    
	  *((DWORD*)(PrimerBuf + (x + i)*baitInPixel + (y + 40)*baitInStrokePoverhn)) = RGB(212, 106, 106);//RED   пиксели (!!! макрос RGB() "ошибается" на DirectDraw поверхности)
      *((DWORD*)(PrimerBuf+(x+i)*baitInPixel+(y+50)*baitInStrokePoverhn))=0x00FF0000;  //RED   пиксели 
	  *((DWORD*)(PrimerBuf + (x + i)*baitInPixel + (y + 60)*baitInStrokePoverhn)) = RGB(85, 170, 85);//GREEN пиксели
      *((DWORD*)(PrimerBuf+(x+i)*baitInPixel+(y+70)*baitInStrokePoverhn))=0x0000FF00;  //GREEN пиксели
	  *((DWORD*)(PrimerBuf + (x + i)*baitInPixel + (y + 80)*baitInStrokePoverhn)) = RGB(0, 51, 51);//BLUE  пиксели (!!! макрос RGB() "ошибается" на DirectDraw поверхности)
      *((DWORD*)(PrimerBuf+(x+i)*baitInPixel+(y+90)*baitInStrokePoverhn))=0x000000FF;  //BLUE  пиксели

      //POINTER (указатель) PrimerBuf == имя одномерного 
	  //массива байт == UCHAR (!!!   PrimerBuf[...]=....;   !!!)
	  //(т.е. четыре соседних элемента массива задают цвет одного пикселя == 32бита) 
	  //Белая строка (старшие 8 бит 4-х байтного пикселя не задаем): 
            PrimerBuf[((x+i)*baitInPixel+0)+(y+120)*baitInStrokePoverhn]=0xFF;//BLUE 
            PrimerBuf[((x+i)*baitInPixel+1)+(y+120)*baitInStrokePoverhn]=0xFF;//GREEN
            PrimerBuf[((x+i)*baitInPixel+2)+(y+120)*baitInStrokePoverhn]=0xFF;//RED
          //RED строка (старшие 8 бит 4-х байтного пикселя не задаем): 
            PrimerBuf[((x+i)*baitInPixel+0)+(y+130)*baitInStrokePoverhn]=0x00;//BLUE 
            PrimerBuf[((x+i)*baitInPixel+1)+(y+130)*baitInStrokePoverhn]=0x00;//GREEN
            PrimerBuf[((x+i)*baitInPixel+2)+(y+130)*baitInStrokePoverhn]=0xFF;//RED
          //GREEN строка (старшие 8 бит 4-х байтного пикселя не задаем): 
            PrimerBuf[((x+i)*baitInPixel+0)+(y+140)*baitInStrokePoverhn]=0x00;//BLUE 
            PrimerBuf[((x+i)*baitInPixel+1)+(y+140)*baitInStrokePoverhn]=0xFF;//GREEN
            PrimerBuf[((x+i)*baitInPixel+2)+(y+140)*baitInStrokePoverhn]=0x00;//RED
          //BLUE строка:
            PrimerBuf[((x+i)*baitInPixel+0)+(y+150)*baitInStrokePoverhn]=0xFF;//BLUE 
            PrimerBuf[((x+i)*baitInPixel+1)+(y+150)*baitInStrokePoverhn]=0x00;//GREEN
            PrimerBuf[((x+i)*baitInPixel+2)+(y+150)*baitInStrokePoverhn]=0x00;//RED
	 }

// */
	 pPrimarySurface->Unlock(NULL);//прекращаем блокирование доступа к ПЕРВИЧНОЙ поверхности
    }
   break;//конец обработки WM_KEYDOWN нажатия клавиши 'F1' (выходим из switch на конец WndProc())
  /****************************************************************************/
   case WM_PAINT://Начальный вывод в рабочую область классического окна и обработка дефектных участков
    static PAINTSTRUCT ps; //Структура для работы BeginPaint()
    hDC = BeginPaint(hWnd,&ps);//  в начале WM_PAINT
	SetTextColor(hDC, RGB(212, 154, 106));//Цвет букв текста
        //SetBkColor(hDC, RGB(255,0,255)); //Цвет фона символов 
          SetBkMode(hDC,TRANSPARENT);      //Фон символов не выводить
          pStr="ВЫВОД в классическое Windows-окно (строки + желтый текст ниже):";
          TextOut(hDC,10,5,pStr,strlen(pStr));//Вывод текста
    	  //Линии средствами GDI макросом RGB() (На классическом окне RGB() выводит цвет правильный !!!)
		  {int X=10,Y=27;
           for(int I=0;I<300;I++)//!!!в DirectDraw они выглядят иначе!!! 
		   {
			   SetPixel(hDC, (X + I), Y, RGB(255, 209, 170));//белая линия
			   SetPixel(hDC, (X + I), Y + 7, RGB(255, 209, 170));//белая линия
			   SetPixel(hDC, (X + I), Y + 14, RGB(212, 106, 106));//RED линия
			   SetPixel(hDC, (X + I), Y + 21, RGB(212, 106, 106));//RED линия
			   SetPixel(hDC, (X + I), Y + 28, RGB(136, 204, 136));//GREEN линия
			   SetPixel(hDC, (X + I), Y + 35, RGB(136, 204, 136));//GREEN линия
			   SetPixel(hDC, (X + I), Y + 42, RGB(64, 127, 127));//BLUE линия
			   SetPixel(hDC, (X + I), Y + 49, RGB(64, 127, 127));//BLUE линия
		   }
          }   
		  pStr="Для перевода окна в режим DirectDraw управления нажмите клавишу 'F1' (Выход: Alt-F4)";
          TextOut(hDC,10,80,pStr,strlen(pStr));//Вывод текста
		  pStr="Для ВЫВОДА DirectDraw данных нажимайте 'F1' любое количество раз (Выход: Alt-F4)";
          TextOut(hDC,10,100,pStr,strlen(pStr));//Вывод текста
    EndPaint(hWnd,&ps); // !!! Обязательно в конце WM_PAINT
   break;
   /*************************************************************************/ 
   case WM_DESTROY: //зачистка VRAM, RAM перед завершением работы программы
       if (pPrimarySurface!=NULL)
        {//Уничтожаем в VRAM массив первичной поверхности(рабочей части окна)
           pPrimarySurface->Release();
           pPrimarySurface = NULL;
	}
         //Уничтожение в RAM экзепляра класса DirectDraw (в heap-е)
           pDirectDraw7->Release();
           pDirectDraw7=NULL;
           PostQuitMessage(0);//посылка сообщения WM_QUIT == окончание работы
   break; 
   default: return(DefWindowProc(hWnd,uMsgName,wParam,lParam));
  }
  return 0;
} //конец оконной функции WndProc()
/***************************************************************************/

//ОСНОВНАЯ ФУНКЦИЯ проекта == точка входа в Windows-проект
int APIENTRY WinMain(HINSTANCE hInstance,HINSTANCE hPrevInstance,
                         LPSTR lpszCmdLine,int iCmdShow)
{ static char szWndClass[]="WndClass for PPKK_Min.cpp";//имя класса окна
  static char szWndTitle[]="Простейший проект DirectDraw (Мое DirectX7)"; 
/***************************************************************************/
  WNDCLASSEX wndclass;//экземпляр структуры для параметров класса окна(12 полей) 
     wndclass.lpszClassName = szWndClass; //имя класса окна (текст, см. выше)
     wndclass.lpfnWndProc = WndProc;      //имя оконной функции
     wndclass.style=CS_SAVEBITS|CS_VREDRAW|CS_HREDRAW|CS_DBLCLKS;//стиль окна
     wndclass.hInstance = hInstance;      // адрес запущенного экземпляра данного приложения      
     wndclass.hIcon = LoadIcon( NULL, IDI_APPLICATION );// иконка ...
     wndclass.hIconSm = LoadIcon( NULL, IDI_QUESTION ); // иконка ...
     wndclass.hCursor = LoadCursor( NULL, IDC_ARROW );  // форма курсора
   //wndclass.hbrBackground =(HBRUSH) GetStockObject(BLACK_BRUSH);//черный фон = NULL
   //  wndclass.hbrBackground =(HBRUSH)( COLOR_GRAYTEXT + 1 );//серый фон рабочей области окна  
	 wndclass.hbrBackground =(HBRUSH)( COLOR_WINDOW + 1 );//белый фон рабочей области окна
     wndclass.lpszMenuName = NULL;
     wndclass.cbClsExtra = 0;
     wndclass.cbWndExtra = 0;
     wndclass.cbSize=sizeof(wndclass);
  RegisterClassEx(&wndclass);//теперь  значения из wndclass приняты к исполнению 
/****************************************************************************/
  HWND hWnd=hWndG=  //адрес (handle) кода окна в RAM (создание окна в RAM (без вывода на экран)
     CreateWindowEx(0, //WS_EX_TOPMOST, //стиль окна (дополнительный)
                    szWndClass,         //имя класса окна (текст, см. выше)
	                szWndTitle,	        //текст заголовка в окне (текст, см. выше)
                    WS_POPUP,           //стиль окна ==  без элементов обрамления окна
                                        //рабочая область окна == весь экран
				  //WS_OVERLAPPEDWINDOW,//стиль - классическое окна
                  //Задание размеров рабочей области окна (первичной поверхности)
                    0,0,		//координаты X и Y левый верхн угол окна
                    GetSystemMetrics(SM_CXSCREEN),//ширина окна в pixel
                    GetSystemMetrics(SM_CYSCREEN),//высота окна в pixel
                //  CW_USEDEFAULT, CW_USEDEFAULT,CW_USEDEFAULT, CW_USEDEFAULT, 
                    NULL,		//дескриптор родительского окна
                    NULL,		//дескриптор меню
                    hInstance,  //описатель (==адрес) данного экземпляра приложения
                    NULL);		//дополнительные данные
/****************************************************************************/
    ShowWindow(hWnd,iCmdShow); // изображение окна на экране  
    UpdateWindow(hWnd);        // посылка первого сообщения WM_PAINT
/****************************************************************************/
   MSG msg;   // экземпляр структуры для получаемых сообщений
   //"Бесконечный" цикл обработки сообщений - вызовов оконной функции
   while(GetMessage(&msg,NULL,0,0))
    {TranslateMessage(&msg);
     DispatchMessage(&msg);
    }//выход из цикла при получении сообщения именем WM_QUIT
  return msg.wParam; 
} // конец WinMain
