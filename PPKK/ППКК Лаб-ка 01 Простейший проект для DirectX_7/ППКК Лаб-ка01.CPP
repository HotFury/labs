//     ПРИМЕР ознакомительного Windows-приложения      // 
// на базе методов DirectDraw из библиотеки DirectX    //
//              доц. Е.А. Лобода                       //
//------------------------------------------------------
// Выход из просмотра изображения: по нажатию клавиши VK_SPACE(пробел)

#define STRICT              // для возможности работы и с MFC
#define WIN32_LEAN_AND_MEAN // для ускорения запуска процесса

#include <windows.h>
//#include <windowsx.h>     // макросы обработки Win-сообщений

#include <ddraw.h>    //для доступа к DirectX
#include <mmsystem.h> //для доступа к Multimedia API's DirectX

#define ClassName "DX_Window"            // имя класса будущего окна
#define AppName "DirectDraw Application" // текст заголовка окна

HWND hWndG;  //глобальная (для использования значения hWnd в других функциях)

//!!!!! Объявления pointer's на интерфейсы DirectDraw !!!!!!!!!
LPDIRECTDRAW7 pDD; 
LPDIRECTDRAWSURFACE7 pPrimarySurface;
LPDIRECTDRAWSURFACE7 pPicFrames;
LPDIRECTDRAWSURFACE7 pFPS;
LPDIRECTDRAWSURFACE7 pBackBuffer;
LPDIRECTDRAWPALETTE  pDDPal;

//имя выводимого на экран графического файла 
char* pFileName ="Example.bmp"; 
int frame=0;

//вспоогательные константы для программы
#define MAX_HEIGHT 480  // желаемое  разрешение экрана 
#define MAX_WIDTH 640   // == 
#define COLOR_DEPTH 8   // 8бит/pixel
#define TRASPARENT_COLOR 0xFF//для "цветовых ключей"

#define FRAME_HEIGHT 240 // размеры внеэкранной поверхности 
#define FRAME_WIDTH 320  // ==

#define FPS_HEIGHT 40    // размеры внеэкранной поверхности 
#define FPS_WIDTH 60     // ==

// Прототипы функций программы
BOOL InitDirectDraw (HWND);    //Инициализация DirectDraw
void ErrorHandle(HWND,LPCTSTR);//Функция, вызываемая в случае ошибки
BOOL CreateSurfaces();         //Создание поверхностей 
LPDIRECTDRAWPALETTE CreateDirectDrawPalette(LPDIRECTDRAW7);//== палитры
BOOL PrepareSurfaces();        //Подготовка поверхностей к выводу      
BOOL LoadBMP(LPDIRECTDRAWSURFACE7,char*);//Загрузка изобр. из BMP-файла
void PrepareFrame();           //Проверка кадра на "потерю"
BOOL ClearSurface(LPDIRECTDRAWSURFACE7);//Отчистка поверхности
void DrawFrame();              //Вывод изображения на экран
void RemoveDirectDraw();       //Отчистка всех интерфейсов, связи с DirectDraw
LRESULT CALLBACK WndProc(HWND, UINT, WPARAM, LPARAM);//оконная функция проекта

// Точка входа в программу
int WINAPI WinMain(HINSTANCE hInst, HINSTANCE hPrevInst, 
				   LPSTR lpszCmdLine, int nCmdShow)
{
	WNDCLASSEX wndClass;
	HWND hWnd;
	MSG msg;
//-------------------------------------------------------
	wndClass.cbSize       =sizeof(wndClass);
	wndClass.style        =CS_HREDRAW|CS_VREDRAW;
	wndClass.lpfnWndProc  =WndProc;
	wndClass.cbClsExtra   =0;
	wndClass.cbWndExtra   =0;
	wndClass.hInstance    =hInst;
	wndClass.hIcon        =LoadIcon(NULL,IDI_WINLOGO);
	wndClass.hCursor      =LoadCursor(NULL,IDC_ARROW);
	wndClass.hbrBackground=NULL;//Обратите внимание!!!
	wndClass.lpszMenuName =NULL;
	wndClass.lpszClassName=ClassName;
	wndClass.hIconSm      =LoadIcon(NULL,IDI_WINLOGO);

	RegisterClassEx(&wndClass);
//------------------------------------------------------
  hWndG=hWnd=CreateWindowEx(
		0, //WS_EX_TOPMOST,//Дополнительный стиль окна
		ClassName,	//Класс окна
		AppName,	//Текст заголовка
		WS_POPUP,	//Стиль окна
		0,0,		//Координаты X и Y
		GetSystemMetrics(SM_CXSCREEN),
		GetSystemMetrics(SM_CYSCREEN),//Ширина и высота
		NULL,		//Дескриптор родительского окна
		NULL,		//Дескриптор меню
		hInst,		//Описатель экземпляра
		NULL);		//Дополнительные данные
//--------------------------------------------------------
	ShowWindow(hWnd,nCmdShow);
	UpdateWindow(hWnd);
//---------------------------------------------------------
   while(GetMessage(&msg,NULL,0,0)) 
   {TranslateMessage( &msg );
    DispatchMessage( &msg );  
   }
   return msg.wParam;
}  // конец WinMain
//---------------------------------------------------------
//Оконная функция
LRESULT CALLBACK WndProc(HWND hWnd, UINT msg, 
			 WPARAM wParam, LPARAM lParam)
{
 HDC hDC;
 switch(msg)
 {
   case WM_CREATE:
//Инициализация компонент, связанных с DirectDraw
//сразу после выполнения CreateWindowEx()
	if (!InitDirectDraw(hWnd))
		return FALSE;
   break;

//При нажатии пробела прекратить работу программы
   case WM_KEYDOWN:
    if(wParam==VK_SPACE) DestroyWindow(hWnd);
   break;
	 
// Работа по WM_PAINT: 
	case WM_PAINT:
     PAINTSTRUCT ps;  
     hDC = BeginPaint( hWnd, &ps );// !!! Обязательно в начале WM_PAINT
   //Прорисовка изображения на экране
     DrawFrame();
	 EndPaint( hWnd, &ps );      // !!! Обязательно в конце WM_PAINT
    break;

  //Убрать в RAM после себя
 case WM_DESTROY:
	RemoveDirectDraw();
	PostQuitMessage(0);
    break;

 default: return DefWindowProc(hWnd,msg,wParam,lParam);
 }
return 0;
}
//---------------------------------------------------------
//Инициализация DirectDraw
//
BOOL InitDirectDraw (HWND hwnd)
{
	//Переменная для возвращаемых кодов
	HRESULT hRet;

	//Обнуляем все интерфейсы
	pPrimarySurface=NULL;
	pBackBuffer=NULL;
	pDDPal=NULL;

	pPicFrames=NULL;

	hWndG=hwnd; 

	//Создание интерфейса IDirectDraw7
	hRet=DirectDrawCreateEx(NULL, (VOID**)&pDD, IID_IDirectDraw7, NULL);
	if (hRet!=DD_OK)
	{
		ErrorHandle(hWndG, "DirectDrawCreate");
		return (FALSE);
	}

	//Установка эксклюзивного режима кооперации
	hRet=
	pDD->SetCooperativeLevel(hWndG, DDSCL_EXCLUSIVE | DDSCL_FULLSCREEN);
	if (hRet!=DD_OK)
	{
		ErrorHandle(hWndG,"SetCooperativeLevel");
		return (FALSE);
	}

	//Установка необходимого видеорежима
	hRet=
	pDD->SetDisplayMode(MAX_WIDTH, MAX_HEIGHT, COLOR_DEPTH, NULL, NULL);
	if (hRet!=DD_OK)
	{
		ErrorHandle(hWndG,"SetDisplayMode");
		return (FALSE);
	}

	//Вызов функции создания поверхностей
	if (!CreateSurfaces())
	{
		ErrorHandle(hWndG, "CreateSurfaces");
		return (FALSE);
	}

	//Вызов функции подготовки поверхностей к работе
	if (!PrepareSurfaces())
	{
		ErrorHandle(hWndG, "ConfigureSurfaces");
		return (FALSE);
	}
	return (TRUE);
}
//---------------------------------------------------------
//Функция, вызываемая в случае ошибки
//
void ErrorHandle(HWND hwnd, LPCTSTR szError)
{
	//Переменная, которая будет содержать строку с текстом ошибки
	char szErrorMessage[255];
	//Перед тем как вывести сообщение,
	//корректно завершим работу DirectDraw
	RemoveDirectDraw();
	//Скроем основное окно
	ShowWindow(hwnd, SW_HIDE);
	//Выведем сообщение об ошибке
	wsprintf(szErrorMessage, "Программа прервана\nОшибка в %s", szError);
	MessageBox(hwnd, szErrorMessage, AppName, MB_OK);
	//Уничтожим окно
	DestroyWindow(hwnd);
}
//---------------------------------------------------------
//Создание поверхностей
//
BOOL CreateSurfaces()
{
	//Объявление необходимых для многих функций DirectDraw
	//структур и переменных
	DDSURFACEDESC2 ddSurfaceDesc;
	DDSCAPS2 ddsCaps;
	HRESULT hRet;

	//Отчистка структуры от "мусора" и установка поля ее размера
	ZeroMemory(&ddSurfaceDesc, sizeof(ddSurfaceDesc));
	ddSurfaceDesc.dwSize=sizeof(ddSurfaceDesc);

	//Установка необходимых полей структуры
	ddSurfaceDesc.dwFlags=DDSD_CAPS | DDSD_BACKBUFFERCOUNT;
	ddSurfaceDesc.ddsCaps.dwCaps=
		DDSCAPS_PRIMARYSURFACE | DDSCAPS_FLIP | DDSCAPS_COMPLEX;
	ddSurfaceDesc.dwBackBufferCount=1;

	//Создание поверхности
	hRet=pDD->CreateSurface(&ddSurfaceDesc,	&pPrimarySurface, NULL);
	if (hRet!=DD_OK)
		return (FALSE);

	//Создание вторичного буфера
	ZeroMemory(&ddsCaps, sizeof(ddsCaps));
	ddsCaps.dwCaps=DDSCAPS_BACKBUFFER;
	hRet=pPrimarySurface->GetAttachedSurface(&ddsCaps, &pBackBuffer);
	if(hRet!=DD_OK)
		return (FALSE);

	//Создание внеэкранных поверхностей
	
		ZeroMemory(&ddSurfaceDesc, sizeof(ddSurfaceDesc));
		ddSurfaceDesc.dwSize=sizeof(ddSurfaceDesc);
		ddSurfaceDesc.dwFlags=DDSD_CAPS | DDSD_HEIGHT | DDSD_WIDTH;
		ddSurfaceDesc.ddsCaps.dwCaps=DDSCAPS_OFFSCREENPLAIN;
		ddSurfaceDesc.dwHeight=FRAME_HEIGHT;
		ddSurfaceDesc.dwWidth=FRAME_WIDTH;
		hRet=pDD->CreateSurface(&ddSurfaceDesc, &pPicFrames, NULL);
		if(hRet!=DD_OK)
			return (FALSE);
	
		ZeroMemory(&ddSurfaceDesc, sizeof(ddSurfaceDesc));
		ddSurfaceDesc.dwSize=sizeof(ddSurfaceDesc);
		ddSurfaceDesc.dwFlags=DDSD_CAPS |
			DDSD_HEIGHT | DDSD_WIDTH;
		ddSurfaceDesc.ddsCaps.dwCaps=DDSCAPS_OFFSCREENPLAIN;
		ddSurfaceDesc.dwHeight=FPS_HEIGHT;
		ddSurfaceDesc.dwWidth=FPS_WIDTH;
		hRet=pDD->CreateSurface(&ddSurfaceDesc, &pFPS, NULL);
		if(hRet!=DD_OK)
			return (FALSE);

	//Установка параметров структуры с "цветовыми ключами"
	DDCOLORKEY ddColorKey;
	ddColorKey.dwColorSpaceLowValue=TRASPARENT_COLOR;
	ddColorKey.dwColorSpaceHighValue=TRASPARENT_COLOR;

	//Установка "цветовых ключей" для всех внеэкранных поверхностей
	pPicFrames->SetColorKey(DDCKEY_SRCBLT, &ddColorKey);

	pFPS->SetColorKey(DDCKEY_SRCBLT, &ddColorKey);
	return (TRUE);
}
//---------------------------------------------------------
//Создание палитры
//
LPDIRECTDRAWPALETTE CreateDirectDrawPalette(LPDIRECTDRAW7 pDD)
{
	//Объявление интерфейсов и структур для работы с палитрой
	LPDIRECTDRAWPALETTE pDirectDrawPal;
	PALETTEENTRY palEntries[256];
	HRESULT hRet;
	LPRGBQUAD pColorTable;
	UINT uMemNeed=sizeof(RGBQUAD)*256;
	DWORD nBytesRead;

	//Открытие графического файла, содержащего палитру
	HANDLE hFile=CreateFile(pFileName, GENERIC_READ,
			FILE_SHARE_READ, NULL, OPEN_EXISTING, 0, NULL);
	if (hFile==INVALID_HANDLE_VALUE)
	{	pDirectDrawPal=NULL;
		return (pDirectDrawPal);
	}
	//Выделение памяти под файловую палитру
	pColorTable= (LPRGBQUAD)malloc(uMemNeed);
	//Установка указателя файла на начало палитры
   SetFilePointer(hFile, 
   sizeof(BITMAPFILEHEADER)+sizeof(BITMAPINFOHEADER), NULL, FILE_BEGIN);
	//Чтение палитры из файла
	ReadFile(hFile, (LPVOID)pColorTable, uMemNeed, &nBytesRead, NULL);
	//Закрытие графического файла
	CloseHandle(hFile);

	//Перевод палитры из RGBQUAD в RGBTRIPPLE
	for (int x=0;x<256;++x)
	{	palEntries[x].peRed=pColorTable[x].rgbRed;
		palEntries[x].peBlue=pColorTable[x].rgbBlue;
		palEntries[x].peGreen=pColorTable[x].rgbGreen;
	}
	//Создание палитры DirectDraw
	hRet=pDD->CreatePalette(DDPCAPS_8BIT | DDPCAPS_ALLOW256,
		palEntries, &pDirectDrawPal, NULL);
	if (hRet!=DD_OK) pDirectDrawPal=NULL;
	//Освобождение памяти
	free(pColorTable);
	return (pDirectDrawPal);
}
//---------------------------------------------------------
//Подготовка поверхностей к выводу
BOOL PrepareSurfaces()
{
	//Создание палитры DirectDraw
	pDDPal=CreateDirectDrawPalette(pDD);
	if (pDDPal==NULL)
		return (FALSE);

	//Присваивание палитры первичной поверхности
	pPrimarySurface->SetPalette(pDDPal);

	//Загрузка графических данных из файлов на внеэкранные поверхности
	if (!LoadBMP(pPicFrames, pFileName))return (FALSE);
	
	ClearSurface(pFPS);
	return (TRUE);
}
//---------------------------------------------------------
//Загрузка изображения из BMP-файла
//
BOOL LoadBMP(LPDIRECTDRAWSURFACE7 pSurface, char* filename)
{
	//Объявление переменных, необходимых для чтения данных из BMP-файла
	BYTE* pBmp;
	DWORD dwBmpSize;
	DWORD dwFileLength;
	DWORD nBytesRead;

	BITMAPINFO* pBmpInfo;
	BYTE*		pPixels;
	HDC hdc;

	//Открытие файла с графическими данными
	HANDLE hFile=CreateFile(filename, GENERIC_READ,
			FILE_SHARE_READ, NULL,OPEN_EXISTING, 0, NULL);
	if (hFile==INVALID_HANDLE_VALUE)
		return (FALSE);

	//Получение размера файла и размера данных
	dwFileLength=GetFileSize (hFile, NULL) ;
	dwBmpSize=dwFileLength-sizeof(BITMAPFILEHEADER);

	//Выделение памяти под данные
	pBmp=	(BYTE*) malloc(dwBmpSize);
	SetFilePointer(hFile, sizeof(BITMAPFILEHEADER), NULL, FILE_BEGIN);

	//Чтение файла с данными
	ReadFile(hFile, (LPVOID)pBmp, dwBmpSize, &nBytesRead, NULL);
	CloseHandle(hFile);

	pBmpInfo=(BITMAPINFO*)pBmp;
	pPixels=pBmp+sizeof(BITMAPINFOHEADER)+sizeof(RGBQUAD)*256;

	//Получение заголовка контекста устройства внеэкранной поверхности
	if ((pSurface->GetDC(&hdc)) == DD_OK)
	{
	//Копирование графических данных из памяти
	//на внеэкранную поверхность средствами GDI
	StretchDIBits(hdc , 0, 0, FRAME_WIDTH, FRAME_HEIGHT, 0, 0, 
		   FRAME_WIDTH, FRAME_HEIGHT, pPixels, pBmpInfo, 0, SRCCOPY);
	pSurface->ReleaseDC(hdc);
	}
	//Освобождение памяти
	free(pBmp);
	return (TRUE);
}
//---------------------------------------------------------
//Проверка кадра на "потерю"
void PrepareFrame()
{
	//Проверка на "потерю" первичной поверхности
	if (pPrimarySurface->IsLost())
	{
		//Восстановление первичной поверхности и вторичного буфера
		pPrimarySurface->Restore();
		pBackBuffer->Restore();

		//Проверка внеэкранных поверхностей на "потерю"
			if (pPicFrames->IsLost())
			{	//Восстановление внеэкранных поверхностей
				//и заполнение их данными из файлов
				pPicFrames->Restore();
				LoadBMP(pPicFrames, pFileName);
			}
		
		if (pFPS->IsLost())
		{	pFPS->Restore();
			ClearSurface(pFPS);
		}
	}
}
//---------------------------------------------------------
//Отчистка поверхности
//
BOOL ClearSurface(LPDIRECTDRAWSURFACE7 pSurface)
{
	DDSURFACEDESC2 ddSurfaceDesc;
	HRESULT hRet;

	ZeroMemory(&ddSurfaceDesc,sizeof(ddSurfaceDesc));
	ddSurfaceDesc.dwSize=sizeof(ddSurfaceDesc);

	hRet=pSurface->Lock(NULL,&ddSurfaceDesc,
			DDLOCK_SURFACEMEMORYPTR | DDLOCK_WAIT,NULL);
	if(FAILED(hRet))
	{ return (FALSE);
	}
	UINT surfaceWidth=ddSurfaceDesc.lPitch;
	UINT surfaceHeight=ddSurfaceDesc.dwHeight;

	char *buf=(char*)ddSurfaceDesc.lpSurface;
	ZeroMemory(buf,surfaceWidth*surfaceHeight);
	pSurface->Unlock(NULL);
	return (TRUE);
}
//---------------------------------------------------------
//Вывод изображения на экран
void DrawFrame()
{
	RECT rPic;
	//Подготовка поверхностей
	PrepareFrame();// проверка кадра на потерю 
	ClearSurface(pBackBuffer);

	//Установка размеров копируемого блока данных
	SetRect(&rPic, 0, 0, FRAME_WIDTH, FRAME_HEIGHT);

	//Копирование графических данных с внеэкранной поверхности
	//на вторичный буфер
	pBackBuffer->BltFast((MAX_WIDTH-FRAME_WIDTH)/2,
	     (MAX_HEIGHT-FRAME_HEIGHT)/2,pPicFrames, &rPic, 
		 DDBLTFAST_SRCCOLORKEY | DDBLTFAST_WAIT);

	SetRect(&rPic, 0, 0, FPS_WIDTH, FPS_HEIGHT);
	pBackBuffer->BltFast((MAX_WIDTH-FPS_WIDTH)/2, 20, 
		 pFPS, &rPic, DDBLTFAST_SRCCOLORKEY | DDBLTFAST_WAIT);

	//Переключение поверхностей
	pPrimarySurface->Flip(NULL, DDFLIP_WAIT);
}
//--------------------------------------------------
//Отчистка всех интерфейсов, связанных с DirectDraw
void RemoveDirectDraw()
{
	//Проверяем,    существует ли интерфейс IDirectDraw7
	if (pDD != NULL)
	{
		//Проверяем, существует ли интерфейс первичной поверхности
		if (pPrimarySurface!=NULL)
		{
			//Уничтожаем интерфейс первичной поверхности
			pPrimarySurface->Release();
			pPrimarySurface = NULL;
		}
		//Проверка на существование внеэкранной поверхности
			if (pPicFrames!=NULL)
			{ //Уничтожение внеэкранной поверхности
				pPicFrames->Release();
				pPicFrames=NULL;
			}
		//Проверка на существование интерфейса палитры
		if(pDDPal!=NULL)
		{ //Уничтожение интерфейса палитры
			pDDPal->Release();
			pDDPal=NULL;
		}
		//Уничтожение интерфейса DirectDraw
		pDD->Release();
		pDD=NULL;
	}
}