// Курс: Проектирование программных компонент и комплексов (ППКК)
// Лектор: доц. Евгений Александрович Лобода
//"Вывод окна, управляемого методами DirectDraw библиотеки DirectX v.7" 
/***************************************************************************/
// ЗАДАНИЕ: по нажатию на клавиатуре клавиши 'F1' : 
// 1 - вывести  информацию о параметрах используемого адаптера (...).
// 2 - вывести на экран и в файл перечень всех поддерживаемых адаптером режимов с указанием для каждого:
//     разрешения (по X и  по Y), бит в значении цвета пикселя, частоты развертки экрана
/***************************************************************************/
// НИЖЕ: Минимальная API-программа проекта для Win98/NT/2000/XP/Win2003,          
// состоящая из 2-х функций: оконной функции WndProc() и главной функции WinMain() 
/***************************************************************************/
#define WIN32_LEAN_AND_MEAN // для ускорения запуска процесса
#include <windows.h>        // для доступа к API функциям Windows
#include <stdio.h>          // для доступа к sprintf()
#include <ddraw.h>          // для доступа к DirectX
#include <mmsystem.h>       // для доступа к Multimedia API's DirectX
#include <winbase.h>        // для записи в файл

//ГЛОБАЛЬНЫЕ ОБЪЯВЛЕНИЯ для использования во всех функциях проекта
static LPDIRECTDRAW7 pDirectDraw7=NULL;   // для pointer-а на класс DirectDraw
static LPDIRECTDRAWSURFACE7 pPrimarySurface=NULL;// для pointer-а на первичную поверхность (Surface)
static DDSURFACEDESC2 DDSurfaceDesc={NULL};//структура для параметров поверхности c обнулением ее полей !!!
static HDC hDC;      //для GDI контекста вывода на экран
static int nF1=0;//счетчик подсчета числа нажатий клавиши 'F1'
       HANDLE  hMyOutFile;//для работы с файлом
static DWORD WritenBites=NULL;//количества записанных байт
       HRESULT hRet; //Переменная для возвращаемых функциями кодов ошибок
                     //[ использовать (обычно) не будем, НО НАДО! ]
       char Str[150],*pStr=NULL;// для формирования текстовых строк
       HWND hWndG;   // для использования значения hWnd в разных функциях проекта

/****************************************************************************/
//ФУНКЦИЯ1 "обратного вызова"(Callback) для обеспечения работы метода DirectDrawEnumerateEx() (используется в WndProc() )
BOOL WINAPI DDEnumCallbackEx(GUID FAR *lpGUID,//значение GUID идентификатора объекта DirectDraw 
							 LPSTR lpDriverDescription,//строка с описанием  очередного найденного драйвера
							 LPSTR lpDriverName,//строка с именем  очередного найденного драйвера 
							 LPVOID lpContext,//
							 HMONITOR hm //номер описываемого монитора (0 = текущий) 
							)
 {  static int N=0;//для смещения текста по оси y
  //Блокируем ПЕРВИЧНУЮ поверхность (== рабочую область экрана)
    hRet=pPrimarySurface->Lock(NULL,          //блокируем ВСЮ ПЕРВИЧНУЮ поверхность (можно только RECT)
                               &DDSurfaceDesc,//адрес структуры, в которую запишет текущие параметрами заблокированной ПЕРВИЧНОЙ поверхности (выводимой сейчас) - объявлена в ГЛОБАЛЬНЫХ переменных 
                               DDLOCK_SURFACEMEMORYPTR | //флаг == получить поинтер на начало заблокированной поверхности (прямоугольника)
                               DDLOCK_WAIT,              //ожидать до получения реального доступа к поверхности  
                               NULL);         // не используется
  //Получение GDI контекста для вывода на ПЕРВИЧНУЮ поверхность
    hRet=pPrimarySurface->GetDC(&hDC);
          
  //ВЫВОД текста средствами GDI (выводим текущие значения, полученные функцией DDEnumCallbackEx() )
    sprintf(Str,"lpGUID=%x  DriverDescription=%s  DriverName=%s  Handle to device=%x",
                 lpGUID,    lpDriverDescription,  lpDriverName,  hm);
    TextOut(hDC,20,(150+N),Str,strlen(Str));//вывод очередной записи
  //смещения текста по y для вывода при следующем вызове этой функции ( DDEnumCallbackEx() )
    N=N+20;//20 - шаг смещения текста по y
  
  //Запись этой строки в файл (добавили \n == переход на новую строку вьюверам):
//  static DWORD WritenBites=NULL;//количества записанных байт
    sprintf(Str,"lpGUID=%x  DriverDescription=%s  DriverName=%s  Handle to device=%x \n",
                 lpGUID,    lpDriverDescription,  lpDriverName,  hm);
      WriteFile(hMyOutFile, // дескриптор (хендл) на файл (объявлен глобально)
                Str,        // адрес зписываемой строки
			    strlen(Str),// размер зписываемой строки
				&WritenBites,  // получаем количество реально записанных байт
				NULL           // не используется
               );

//	               );
	



    pPrimarySurface->ReleaseDC(hDC);//удаляем GDI контекст
    pPrimarySurface->Unlock(NULL);//прекращаем блокирование доступа к ПЕРВИЧНОЙ поверхности

    return TRUE; // == продолжать вывод следующих строк 
//  return FALSE;// == прекратить вывод (прекратить вызов этой (Callback) функции - DDEnumCallbackEx)
  };

/****************************************************************************/
//ФУНКЦИЯ2 "обратного вызова"(Callback) для обеспечения работы метода EnumDisplayModes() (используется в WndProc() ) 
HRESULT WINAPI EnumModesCallback2(
                LPDDSURFACEDESC2 lpddSurfaceDesc,//поинтер на структуру параметров очередной из возможных первичных поверхностей  
                LPVOID lpContext //вспомогательные данные из программы проекта(не используем этот канал) 
                				 )
   { static int K=0;//для смещения текста по оси y (в TextOut() )
     static int M=0;//для нумерации режимов
	//Блокируем ПЕРВИЧНУЮ поверхность (== рабочую область экрана)
	  hRet=pPrimarySurface->Lock(
		    NULL,//блокируем ВСЮ поверхность (можно только RECT)
		    &DDSurfaceDesc,//адрес структуры с параметрами РЕАЛЬНОЙ ПЕРВИЧНОЙ поверхности(выводимой сейчас)- объявлена в ГЛОБАЛЬНЫХ переменных 
            DDLOCK_SURFACEMEMORYPTR | //флаг получения поинтера на начало поверхности
			DDLOCK_WAIT,//ожидать до получения реального доступа к поверхности  
			NULL); // не используется
    //Получение GDI контекста для вывода
	  hRet=pPrimarySurface->GetDC(&hDC);

	//ВЫВОД текста средствами GDI (выводим текущие значения, полученные этой функцией )
      M++;//для нумерации режимов
	  sprintf(Str,"Mode  %3d:    %4d x %4d      %2d      %3d      ",//формируем в массиве Str строку вывода 
		                   M,                                       //из очередной структуры возможных значений параметров поверхности
		                   lpddSurfaceDesc->dwWidth,//разрешение по X 
		                   lpddSurfaceDesc->dwHeight,//разрешение по Y
	                       lpddSurfaceDesc->ddpfPixelFormat.dwRGBBitCount,// бит на пиксель
	                       lpddSurfaceDesc->dwRefreshRate);// частота развертки кадров монитора
	  TextOut(hDC,20,(190+K),Str,strlen(Str));//вывод очередной записи
      K=K+20;//20 - шаг смещения текста по y
    
	//Запись этой строки в файл (добавили \n == переход на новую строку вьюверам):
//	  static DWORD WritenBites=NULL;//количества записанных байт
	  sprintf(Str,"Mode  %3d:    %4d x %4d      %2d      %3d\n",//формируем в массиве Str строку вывода 
		                   M,                                   //из очередной структуры возможных значений параметров поверхности
                           lpddSurfaceDesc->dwWidth,//разрешение по X
		                   lpddSurfaceDesc->dwHeight,//разрешение по Y
	                       lpddSurfaceDesc->ddpfPixelFormat.dwRGBBitCount,// бит на пиксель
                           lpddSurfaceDesc->dwRefreshRate);// частота развертки кадров монитора
	  // ЗАПИСЬ очередной строки в файл:
	  WriteFile(hMyOutFile, // дескриптор (хендл) на файл (объявлен глобально)
                Str,        // адрес зписываемой строки
			    strlen(Str),// размер зписываемой строки
				&WritenBites,  // получаем количество реально записанных байт
				NULL           // не используется
               );

      pPrimarySurface->ReleaseDC(hDC);//удаляем GDI контекст
      pPrimarySurface->Unlock(NULL);//прекращаем блокирование доступа к ПЕРВИЧНОЙ поверхности

	  if(Str[0]==0)return DDENUMRET_CANCEL;//прекратить вызов этой (Callback) функции - EnumModesCallback2() 
      return DDENUMRET_OK;//продолжать вывод следующих строк 
    } 


/****************************************************************************/
// ОКОННАЯ ФУНКЦИЯ == действия программы по обработке сообщений в Windows 
LRESULT CALLBACK WndProc(HWND hWnd,UINT uMsgName,WPARAM wParam,LPARAM lParam)
{/*************************************************************************/
 switch(uMsgName) // Обработка сообщения:
 /*************************************************************************/
  {case WM_KEYDOWN://при нажатии любой клавиши(по WM_CREATE лучше, т.к. вып. до появления изображения окна)
    if(wParam==VK_F1)//если нажата клавиша 'F1'
     {  nF1++;//число нажатий клавиши 'F1'
        pStr="DirectDraw7 не обновлялся по этому нажатию F1 ";//для контроля обновления DirectDraw
	  //Создание экземпляра объекта DirectDraw библиотеки виртуальных классов DirectX v.7.0  
      //(Получаем pointer на DirectDraw указанием его интерфейса в DirectX v.7.0)
      if((nF1==1)|(nF1==2))//!!! делаем экземпляр объекта DirectDraw дважды (при первых двух нажатиях 'F1') ???
		 {  pDirectDraw7=NULL;//обнуляем поинтер на экземпляр DirectDraw7 перед обновлением его значения
		  //создание экземпляра (== интерфейса) DirectDraw 
		    hRet=DirectDrawCreateEx(
				   NULL,                 // создавать для текущего графич. адаптера (можно отключить аксерераторы или их эмуляцию !!!)
                   (VOID**)&pDirectDraw7,// получаем pointer входа в DirectDraw7 (доступ к его методам)
                   IID_IDirectDraw7,     // имя нужного интерфейса DirectDraw7
                   NULL);                // не используется
          //Контроль кодов ошибок создания экземпляра объекта DirectDraw  
            if(hRet==DD_OK)pStr="СОЗДАН новый экземпляр объекта DirectDraw7";
            if(hRet!=DD_OK)pStr="Error при создании DirectDraw7";
            if(hRet==DDERR_DIRECTDRAWALREADYCREATED)pStr="DirectDraw7 уже был создан";
         }
      
       //Задание уровня использования видеоадаптера (есть 12 уровней)
//        if(nF1<=2)//!!! делаем только при первых двух нажатиями 'F1'
              {  hRet=pDirectDraw7->SetCooperativeLevel(hWnd,             //хендел на первичное (классическое) Windows окно
                                                        DDSCL_EXCLUSIVE | //монопольноt управление работой графич. адаптера
                                                        DDSCL_FULLSCREEN);//весь экран
                                                      //DDSCL_NORMAL);    // == Windows окно (работает при стиле WS_OVERLAPPEDWINDOW в CreateWindowEx()
                //ВНИМАНИЕ:
                //Изменения уровня использования видеоадаптера функцией SetCooperativeLevel() 
                //требует при повторном входе в оконную функцию обновить экземпляр класса DirectDraw.
              }
//  /*
	  //Задание графического режима вывода на экран 
//     if(nF1<=2)//!!! делаем при первых двух нажатиями 'F1'
            {hRet=pDirectDraw7->SetDisplayMode(1920, // пикселей по x
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
        } 
//  */

//!!!СОЗДАНИЕ ПОВЕРХНОСТИ, подконтрольной средствам DurectX v.7.0:
// Сздадим ПЕРВИЧНУЮ (Primary) поверхность (Surface)
static DDSURFACEDESC2 ddSurfaceDesc={NULL};//объявление структуры для хранения параметров поверхности c обнулением начапьных значений ее полей !!!
  //ZeroMemory(&ddSurfaceDesc, sizeof(ddSurfaceDesc));//обнуление структуры для параметров поверхности
  //Установка необходимых (желаемых) начальных значений полей структуры DDSURFACEDESC2
	ddSurfaceDesc.dwSize=sizeof(ddSurfaceDesc); //поле размера структуры 
	ddSurfaceDesc.dwFlags=DDSD_BACKBUFFERCOUNT | // разрешаем использовать BACKBUFFER
		                  DDSD_CAPS;             // разрешаем использовать вторичные поверхности
	ddSurfaceDesc.ddsCaps.dwCaps= DDSCAPS_PRIMARYSURFACE | // видимой должна быть первичная поверхность
		                          DDSCAPS_COMPLEX |        // == создается комплексная поверхность
	                              DDSCAPS_FLIP;            // разрешается Flip() переключение поверхностей
	ddSurfaceDesc.dwBackBufferCount=1; // будем использовать 1 (один)BACKBUFFER 

	//Создание ПЕРВИЧНОЙ поверхности (поверхности, видимой на экране)
	hRet=pDirectDraw7->CreateSurface(
		                  &ddSurfaceDesc,//адрес структуры с желаемыми параметрами поверхности   
                          &pPrimarySurface,//получаем pointer на поверхность (адрес ее начала в VRAM)
                          NULL);// не используется

//========================================
// ВЫПОЛНЕНИЕ ЗАДАНИЯ Лабораторной работы:
//========================================

//==================================
//ВЫВОД на ПЕРВИЧНУЮ ПОВЕРХНОСТЬ (рабочую область окна)	
//----------------------------------
//1 ВЫВОД (средствами GDI) ТЕКСТА с параметрами текущего режима
// /*
	//Определение параметров текущего режима (== текущих параметров первичной поверхности) 
      hRet=pDirectDraw7->GetDisplayMode(&ddSurfaceDesc);//

    //Блокируем ПЕРВИЧНУЮ поверхность (== рабочую область экрана)
	  hRet=pPrimarySurface->Lock(
            NULL,//блокируем ВСЮ поверхность (можно только часть == RECT)
		    &DDSurfaceDesc,//адрес вспомогательной структуры для хранения параметров РЕАЛЬНОЙ ПЕРВИЧНОЙ поверхности(выводимой сейчас)- структура объявлена в ГЛОБАЛЬНЫХ переменных 
            DDLOCK_SURFACEMEMORYPTR | //флаг = получить поинтер на начало поверхности
			DDLOCK_WAIT,//ожидать до получения реального доступа к поверхности  
			NULL); // не используется
    //Получение GDI контекста для вывода
	  hRet=pPrimarySurface->GetDC(&hDC);// контекст для первичной поверхности
          
	//ВЫВОД текста средствами GDI (выводим параметры текущего режима) 	sprintf(Str,"ВЫВОД в DirectDraw-окно (см. ниже):");
	  sprintf(Str,"ВЫВОД в DirectDraw-окно (см. ниже):");
	  TextOut(hDC,10,90,Str,strlen(Str));//вывод текста 
	  sprintf(Str,"nF1=%d",nF1);
	  TextOut(hDC,420,90,Str,strlen(Str));//номер нажатия на F1 

	  TextOut(hDC,10,110,pStr,strlen(pStr));//вывод текста об обновлении экземпляра DirectDraw
	  sprintf(Str,"pDirectDraw7=%p",pDirectDraw7);
	  TextOut(hDC,420,110,Str,strlen(Str));//вывод текста об обновлении экземпляра DirectDraw

	  sprintf(Str,"Width=%i  Height=%u  PixelFormat=%d  RefreshRate=%d ",
	               ddSurfaceDesc.dwWidth,// разрешение по X
                                 ddSurfaceDesc.dwHeight,// разрешение по Y
	                                    ddSurfaceDesc.ddpfPixelFormat.dwRGBBitCount,// бит на пиксель
										                 ddSurfaceDesc.dwRefreshRate);//частота кадров
	  TextOut(hDC,10,130,Str,strlen(Str));//вывод текста о текущем режиме
	  // контроль изменения адресов поверхностей (Surface):
	  sprintf(Str,"&ddSurfaceDesc=%p  &pPrimarySurface=%p  pPrimarySurface=%p",&ddSurfaceDesc,&pPrimarySurface,pPrimarySurface);
	  TextOut(hDC,420,130,Str,strlen(Str));//вывод текста об обновлении &ddSurfaceDesc и &pPrimarySurface

	pPrimarySurface->ReleaseDC(hDC);//удаляем GDI контекст

	pPrimarySurface->Unlock(NULL);//прекращаем блокирование доступа к ПЕРВИЧНОЙ поверхности

//  /*
//=!!!!=========
//Получение информации о графическом адаптере компьютера (о всех его графических драйверах и присоединенных к нему устройствах (акселераторах) 
	//DirectDrawEnumerateEx() - обычная API (НЕ ТРЕБУЕТ ИСПОЛЬЗОВАНИЯ pDirectDraw7->), объявлена (прототип) в ddraw.h
	//(Для работы DirectDrawEnumerateEx() используется функция обратного вызова DDEnumCallbackEx(), созданная в этой программе)
    if(nF1==3)//выводим по третьему нажатию на F1
	  {hMyOutFile=CreateFile( //объявление файла для вывода результатов анализа поддерживаемых режимов
                                    "DisplayModes.doc",//имя создаваемого файла с перечнем поддерживаемых режимов
                                     GENERIC_WRITE,     //доступ - только для записи
                                   //FILE_SHARE_READ,   //разрешаем одновременное чтение файла другим потоком 
                                     0,                 //разрешаем одновременное чтение файла другим потоком 
                                     NULL,              //атрибут защиты
                                     OPEN_ALWAYS,       //открывать файл всегда 
                                     0,                 //флаг,аттрибут = буфер создавать не надо 
                                     NULL               //не используем
		                    );
        DirectDrawEnumerateEx( DDEnumCallbackEx,//имя функции обратного вызова(для вывода инф об очередном найденном результате поиска)
	                          NULL,//не передаем дополнительную информации, передаваемой из программы непосредственно в функцию обратного вызова ( EnumModesCallbac() )
                            //0    //сообщать о всех драйверах и присоединенных к нему устройствах (акселераторах)
                            //DDENUM_NONDISPLAYDEVICES 
                            //DDENUM_DETACHEDSECONDARYDEVICES 
                              DDENUM_ATTACHEDSECONDARYDEVICES//сообщать о всех драйверах и присоединенных к нему устройствах (акселераторах)  
							  );
       CloseHandle(hMyOutFile);// закрыли файл
      }
// */

//=======================================
// Другие параметры адаптера:
//IDirectDraw7::GetCaps() ==DDCAPS.dwVidMemTotal, DDCAPS.dwVidMemTotal     ddraw.h
//IDirectDraw7::GetMonitorFrequency     ddraw.h
//IDirectDraw7::GetAvailableVidMem()    ddraw.h
//IDirectDraw7::GetDeviceIdentifier()   ddraw.h
//  ID3DXContext::GetDeviceIndex()        d3dxcore.h !!!
//  IDirectInputDevice7::GetDeviceInfo()  dinput.h

//IDirectDraw7::EnumSurfaces                ddraw.h 
//IDirectDraw7::GetGDISurface               ddraw.h
//IDirectDrawColorControl::SetColorControls ddraw.h !!!
//IDirectDrawColorControl::GetColorControls ddraw.h
//IDirectDraw7::WaitForVerticalBlank        ddraw.h
//IDirectDraw7::GetVerticalBlankStatus      ddraw.h
//IDirectDraw7::GetGDISurface               ddraw.h
//IDirectDraw7::GetSurfaceFromDC            ddraw.h  
//IDirectDrawGammaControl::GetGammaRamp     ddraw.h 
//IDirectDrawGammaControl::SetGammaRamp     ddraw.h
//IDirectDraw7::FlipToGDISurface            ddraw.h



//=!!!!==(API с Callback функцией)=======

//Получение информации о поддерживаемых адаптером видеорежимах (всех его драйверах и присоединенных к нему устройствах 
	//(Для работы EnumDisplayModes() используется функция обратного вызова EnumModesCallback2(), созданная в этой программе)
       if(nF1==3)//выводим по 3-му нажатию на F1
	   {  hMyOutFile=CreateFile( //объявление файла для вывода результатов анализа поддерживаемых режимов
                                      "DisplayModes.doc",//имя создаваемого файла с перечнем поддерживаемых режимов
                                      GENERIC_WRITE,     //доступ - только для записи
                                    //FILE_SHARE_READ,   //разрешаем одновременное чтение файла другим потоком 
                                      0,                 //разрешаем одновременное чтение файла другим потоком 
                                       NULL,              //атрибут защиты
                                      OPEN_ALWAYS,       //открывать файл всегда 
                                      0,                 //флаг,аттрибут = буфер создавать не надо 
                                      NULL               //не используем
		                      );
	      hRet=pDirectDraw7->EnumDisplayModes(DDEDM_REFRESHRATES,//перечислять и режимы с различной частотой обновления экрана
                                                  NULL,              //перечислять ВСЕ режимы
                                                  NULL,              //не передаем дополнительную информации, передаваемой из программы непосредственно в функцию обратного вызова ( EnumModesCallback2() )
                                                  EnumModesCallback2 //имя функции обратного вызова(для вывода инф об очередной определенном режиме
                                                 ); 
         //Закрываем файл
           CloseHandle(hMyOutFile);// закрыли файл
	    }
//=======================================

    }//конец [if(wParam==VK_F1)...] обработки нажатия F1 
   break;//конец обработки WM_KEYDOWN (выходим из switch на конец WndProc())
  /****************************************************************************/
   case WM_PAINT://Начальный вывод в рабочую область классического окна и обработка дефектных участков
    static PAINTSTRUCT ps; //структура для работы BeginPaint()
    hDC = BeginPaint(hWnd,&ps);//  в начале WM_PAINT
		  SetTextColor(hDC, RGB(225, 150, 0));//Цвет букв текста
		  SetBkColor(hDC, RGB(0, 135, 135)); //Цвет фона символов 
		  SetBkMode(hDC, OPAQUE);      //Фон символов не выводить
          pStr="ВЫВОД в классическое Windows-окно (желтый текст ниже):";
          TextOut(hDC,10,5,pStr,strlen(pStr));//Вывод текста
          pStr="Для перевода окна в режим DirectDraw управления нажмите клавишу 'F1' (Выход: Alt-F4)";
          TextOut(hDC,10,30,pStr,strlen(pStr));//Вывод текста
		  pStr="Для ВЫВОДА DirectDraw данных нажимайте 'F1' любое количество раз (Выход: Alt-F4)";
          TextOut(hDC,10,50,pStr,strlen(pStr));//Вывод текста
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
     wndclass.hbrBackground =(HBRUSH)( COLOR_GRAYTEXT + 1 );//серый фон рабочей области окна  
   //wndclass.hbrBackground =(HBRUSH)( COLOR_WINDOW + 1 );//белый фон рабочей области окна
     wndclass.lpszMenuName = NULL;
     wndclass.cbClsExtra = 0;
     wndclass.cbWndExtra = 0;
     wndclass.cbSize=sizeof(wndclass);
  RegisterClassEx(&wndclass);//теперь  
/****************************************************************************/
  HWND hWnd=hWndG=  //адрес (handle) кода окна в RAM (создание окна в RAM (без вывода на экран)
     CreateWindowEx(0, //WS_EX_TOPMOST, //стиль окна (дополнительный)
                    szWndClass,         //имя класса окна (текст, см. выше)
	                szWndTitle,	        //текст заголовка в окне (текст, см. выше)
                    WS_POPUP,           //стиль окна ==  без элементов обрамления окна
                                        //рабочая область окна == весь экран
				  //WS_OVERLAPPEDWINDOW,//стиль окна - классическое окна
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
