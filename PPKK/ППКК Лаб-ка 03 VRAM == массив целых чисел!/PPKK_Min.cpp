// ����: �������������� ����������� ��������� � ���������� (����)
// ������: ���. ������� ������������� ������
//"����� ����, ������������ �������� DirectDraw ���������� DirectX v.7" 
/***************************************************************************/
// �������: �� ������� �� ���������� ������� 'F1' : 
// 1 - ������� ��������� �������� ������ �������� (���������� �� X,���������� �� Y,��� � �������,������� ������).
// 2 - ��������� ������ ��������� �������� ������� ������� ������ (���������� ����� ������ ������).
/***************************************************************************/
// ����: ����������� API-��������� ������� ��� Win98/NT/2000/XP/Win2003,          
// ��������� �� 2-� �������: ������� ������� WndProc() � ������� ������� WinMain() 
/***************************************************************************/
#define WIN32_LEAN_AND_MEAN // ��� ��������� ������� ��������
#include <windows.h>        // ��� ������� � API �������� Windows
#include <stdio.h>          // ��� ������� � sprintf()
#include <ddraw.h>          // ��� ������� � DirectX
#include <mmsystem.h>       // ��� ������� � Multimedia API's DirectX

// ���������� ���������� ��� ������������� �� ���� �������� �������
char Str[200],*pStr=NULL;   // ��� ������������ ��������� �����
HWND hWndG;   // ��� ������������� �������� hWnd � ������ �������� �������

/****************************************************************************/
// ������� ������� == �������� ��������� �� ��������� ��������� � Windows 
LRESULT CALLBACK WndProc(HWND hWnd,UINT uMsgName,WPARAM wParam,LPARAM lParam)
{static LPDIRECTDRAW7        pDirectDraw7=NULL;   // ��� pointer-� �� ����� DirectDraw
 static LPDIRECTDRAWSURFACE7 pPrimarySurface=NULL;// ��� pointer-� �� ��������� ����������� (Surface)
        HRESULT hRet; //���������� ��� ������������ ��������� ����� ������
                      //(������������ �� �����, �� ����!)
 static HDC hDC;      //��� GDI ��������� ������ �� �����
 static int nDDraw=0; //������� �������� ����� ���������� ����������� ������ DirectDraw
 static int nF1=0;//������� �������� ����� ������� ������� 'F1'
 
/*************************************************************************/
 switch(uMsgName) // ��������� ���������:
/*************************************************************************/
  {case WM_KEYDOWN://��� ������� ����� �������(�� WM_CREATE �����, �.�. �� ��������� ����������� ����)
    if(wParam==VK_F1)//���� ������ ������� 'F1'
    {  nF1++;//����� ������� ������� 'F1'
     //�������� ���������� ������� DirectDraw ���������� ����������� ������� DirectX v.7  
     //�������� pointer �� DirectDraw ��������� ��� ���������� � DirectX v.7.0
        pStr="DirectDraw7 �� ���������� �� ����� ������� F1 ";//��� �������� ���������� DirectDraw
      //if(nDDraw==0)//??? ������ ������ ���� ��������� ������� DirectDraw ��� ���� �������� 'F1' ???
        if((nDDraw==0)|(nDDraw==1))//!!! ������ ��������� ������� DirectDraw ������ ��� ���� �������� 'F1' ???
	    {//��������� ���������� ����� ����������� ��������
           pDirectDraw7=NULL;
		 //�������� ���������� (����������) DirectDraw 
		 hRet=DirectDrawCreateEx(NULL,// == ������� ��.������� (����� ��������� ������������ ��� �� ��������)
                (VOID**)&pDirectDraw7,// �������� pointer ����� � DirectDraw7 (������ � ��� �������)
                     IID_IDirectDraw7,// ��� ���������� DirectDraw7
                                NULL);// �� ������������
         //�������� �������� ���������� ������� DirectDraw  
           if(hRet==DD_OK)pStr="������ ����� ��������� ������� DirectDraw7";
           if(hRet!=DD_OK)pStr="Error ��� �������� DirectDraw7";
           if(hRet==DDERR_DIRECTDRAWALREADYCREATED)pStr="DirectDraw7 ��� ��� ������";
           nDDraw++;//����� DirectDraw ������
          }
      
       // ������� ������ ������������� ������������� (���� 12 �������)
           hRet=pDirectDraw7->SetCooperativeLevel(hWnd,
           DDSCL_EXCLUSIVE | DDSCL_FULLSCREEN);//����������, ���� �����
       //  DDSCL_NORMAL);// == Windows ���� (�������� ��� ����� WS_OVERLAPPEDWINDOW � CreateWindowEx()
	   // ��������:
	   // ��������� ������ ������������� ������������� �������� SetCooperativeLevel() 
	   // ������� ��� ��������� ����� � ������� ������� �������� ��������� ������ DirectDraw.

// /* 
	  //������� ������������ ������ ������ �� ����� 
	   hRet=pDirectDraw7->SetDisplayMode(1920, // �������� �� x
	                                     1080, // �������� �� y
	                                      32, // ��� �� �������
	                                  //  16, // ��� �� �������
	                                  //   8, // ��� �� �������
                                        NULL, // ������� ������ �������
                                       NULL);// �� ������������
	   //��������:
	   // ���������� SetDisplayMode()���������� (��������)���� ����� ������� �������
	   // (�� ����������� ��������� ���� ������ � SetDisplayMode()).
	   // �������, ��� ���������� ����������� ���������� (���� SetDisplayMode())
	   // ���� �������� ������ ������� F1 (== ����� ����� � ������� �������)
	   // � �������� ����� DirectDraw (��� � ����� SetCooperativeLevel()). 
       
// */

//==================================
//!!!�������� �����������, �������������� ��������� DurectX v.7:
    DDSURFACEDESC2 ddSurfaceDesc={NULL};//��������� ��� ���������� ����������� 
                                        //c ���������� �� ����� !!!
  //ZeroMemory(&ddSurfaceDesc, sizeof(ddSurfaceDesc));//��������� �� "���������"

	//��������� ����������� ��������� �������� ����� ��������� DDSURFACEDESC2
	ddSurfaceDesc.dwSize=sizeof(ddSurfaceDesc); // ���� ������� ��������� 
        ddSurfaceDesc.dwFlags = DDSD_CAPS;
        ddSurfaceDesc.ddsCaps.dwCaps = DDSCAPS_PRIMARYSURFACE;//��������� ������ ��������� �����������
	ddSurfaceDesc.dwBackBufferCount=0;//��������� ����������� BackBuffer �� �����

	//�������� ��������� ����������� (������� �� ������)
	hRet=pDirectDraw7->CreateSurface(&ddSurfaceDesc,//��������� ���������� �����������   
                           &pPrimarySurface,//�������� pointer �� ����������� (����� �� ������ � VRAM)
                           NULL);

//==================================
//����� �� ��������� ����������� (������� ������� ����)	
//----------------------------------
//1 ����� (���������� GDI) ������ � ����������� �������� ������
// /*
	//����������� ���������� �������� ������ (== �����������) 
    hRet=pDirectDraw7->GetDisplayMode(&ddSurfaceDesc);

    //��������� ��������� ����������� (== ������� ������� ������)
	hRet=pPrimarySurface->Lock(NULL,&ddSurfaceDesc, 
	     DDLOCK_SURFACEMEMORYPTR | DDLOCK_WAIT, NULL);
     
    //��������� GDI ��������� ��� ������
	hRet=pPrimarySurface->GetDC(&hDC);
          
	//����� ������ ���������� GDI (������� ��������� �������� ������) 	sprintf(Str,"����� � DirectDraw-���� (��. ����):");
          sprintf(Str,"����� � DirectDraw-���� (��. ����):");
          TextOut(hDC,10,130,Str,strlen(Str));//����� ������
	  sprintf(Str,"nF1=%d",nF1);
	  TextOut(hDC,400,130,Str,strlen(Str));//����� ������� �� F1 

          TextOut(hDC,10,160,pStr,strlen(pStr));//����� ������ � ���������� DirectDraw
	//�������� ��������� ������ ������������ ������� DirectDraw7:
	  sprintf(Str,"pDirectDraw7=%p  &ddSurfaceDesc=%p",pDirectDraw7,&ddSurfaceDesc);
	  TextOut(hDC,400,160,Str,strlen(Str));//����� ������ �� ���������� ������ ���������� DirectDraw

	  sprintf(Str,"Width=%i  Height=%u  PixelFormat=%d  RefreshRate=%d ",
	  ddSurfaceDesc.dwWidth,ddSurfaceDesc.dwHeight,
	  ddSurfaceDesc.ddpfPixelFormat.dwRGBBitCount,ddSurfaceDesc.dwRefreshRate);
	  TextOut(hDC,10,190,Str,strlen(Str));//����� ������ � ������� ������
	  
	//�������� ��������� ������� ������������ (Surface):
	  sprintf(Str,"pPrimarySurface=%p  &pPrimarySurface=%p ",pPrimarySurface,&pPrimarySurface);
	  TextOut(hDC,400,190,Str,strlen(Str));//����� ������ �� ���������� &ddSurfaceDesc � &pPrimarySurface


	pPrimarySurface->ReleaseDC(hDC);//������� GDI ��������

	pPrimarySurface->Unlock(NULL);//���������� ������������ ������� � ��������� �����������

// */

//----------------------------------
//2 ������ ������ � �������� ����������� (����������� == ������ ����� ����� !!!)
//  ������ ����� ������ ������: 

// /*

  //����������� ���������� �������� ������ (��������� �����������) 
    hRet=pDirectDraw7->GetDisplayMode(&ddSurfaceDesc);
    //��������� ��������� ����������� (������� ������� ������)
	hRet=pPrimarySurface->Lock(NULL,&ddSurfaceDesc,
                              DDLOCK_SURFACEMEMORYPTR | DDLOCK_WAIT, NULL);
    
    //���������� pointer-� == ������� ������ � ����������� ������ ������ �����������
	//(� ��� ��� �������� �� ������� == 1����)
	UCHAR *PrimerBuf=(UCHAR *)ddSurfaceDesc.lpSurface;
	                                                  
	//�������� ��������� ��� ����������� �� ����������� 
      int baitInStrokePoverhn=ddSurfaceDesc.lPitch;//���� � ������
      int baitInPixel=ddSurfaceDesc.ddpfPixelFormat.dwRGBBitCount/8;//���� � �������

	//������ ����� ������ ������ (!!! DirectDraw �������� ������ ������� RGB() !!!) 
	int x=10,y=220;//(!!! ��������� ����� ����� � ��� ������ �������� ��������, ���������� � SetDisplayMode() )
    for(int i=0;i<300;i++)
     {*((DWORD*)(PrimerBuf+(x+i)*baitInPixel+(y+00)*baitInStrokePoverhn))=
                     RGB(rand()%256,rand()%256,rand()%256); //��������� ����
      *((DWORD*)(PrimerBuf+(x+i)*baitInPixel+(y+10)*baitInStrokePoverhn))=
                     RGB(rand()%256,rand()%256,rand()%256); //��������� ����
      *((DWORD*)(PrimerBuf+(x+i)*baitInPixel+(y+20)*baitInStrokePoverhn))=RGB(85, 0, 0);//  ����� �������   
      *((DWORD*)(PrimerBuf+(x+i)*baitInPixel+(y+30)*baitInStrokePoverhn))=0x00FFFFFF;//�����   �������    
	  *((DWORD*)(PrimerBuf + (x + i)*baitInPixel + (y + 40)*baitInStrokePoverhn)) = RGB(212, 106, 106);//RED   ������� (!!! ������ RGB() "���������" �� DirectDraw �����������)
      *((DWORD*)(PrimerBuf+(x+i)*baitInPixel+(y+50)*baitInStrokePoverhn))=0x00FF0000;  //RED   ������� 
	  *((DWORD*)(PrimerBuf + (x + i)*baitInPixel + (y + 60)*baitInStrokePoverhn)) = RGB(85, 170, 85);//GREEN �������
      *((DWORD*)(PrimerBuf+(x+i)*baitInPixel+(y+70)*baitInStrokePoverhn))=0x0000FF00;  //GREEN �������
	  *((DWORD*)(PrimerBuf + (x + i)*baitInPixel + (y + 80)*baitInStrokePoverhn)) = RGB(0, 51, 51);//BLUE  ������� (!!! ������ RGB() "���������" �� DirectDraw �����������)
      *((DWORD*)(PrimerBuf+(x+i)*baitInPixel+(y+90)*baitInStrokePoverhn))=0x000000FF;  //BLUE  �������

      //POINTER (���������) PrimerBuf == ��� ����������� 
	  //������� ���� == UCHAR (!!!   PrimerBuf[...]=....;   !!!)
	  //(�.�. ������ �������� �������� ������� ������ ���� ������ ������� == 32����) 
	  //����� ������ (������� 8 ��� 4-� �������� ������� �� ������): 
            PrimerBuf[((x+i)*baitInPixel+0)+(y+120)*baitInStrokePoverhn]=0xFF;//BLUE 
            PrimerBuf[((x+i)*baitInPixel+1)+(y+120)*baitInStrokePoverhn]=0xFF;//GREEN
            PrimerBuf[((x+i)*baitInPixel+2)+(y+120)*baitInStrokePoverhn]=0xFF;//RED
          //RED ������ (������� 8 ��� 4-� �������� ������� �� ������): 
            PrimerBuf[((x+i)*baitInPixel+0)+(y+130)*baitInStrokePoverhn]=0x00;//BLUE 
            PrimerBuf[((x+i)*baitInPixel+1)+(y+130)*baitInStrokePoverhn]=0x00;//GREEN
            PrimerBuf[((x+i)*baitInPixel+2)+(y+130)*baitInStrokePoverhn]=0xFF;//RED
          //GREEN ������ (������� 8 ��� 4-� �������� ������� �� ������): 
            PrimerBuf[((x+i)*baitInPixel+0)+(y+140)*baitInStrokePoverhn]=0x00;//BLUE 
            PrimerBuf[((x+i)*baitInPixel+1)+(y+140)*baitInStrokePoverhn]=0xFF;//GREEN
            PrimerBuf[((x+i)*baitInPixel+2)+(y+140)*baitInStrokePoverhn]=0x00;//RED
          //BLUE ������:
            PrimerBuf[((x+i)*baitInPixel+0)+(y+150)*baitInStrokePoverhn]=0xFF;//BLUE 
            PrimerBuf[((x+i)*baitInPixel+1)+(y+150)*baitInStrokePoverhn]=0x00;//GREEN
            PrimerBuf[((x+i)*baitInPixel+2)+(y+150)*baitInStrokePoverhn]=0x00;//RED
	 }

// */
	 pPrimarySurface->Unlock(NULL);//���������� ������������ ������� � ��������� �����������
    }
   break;//����� ��������� WM_KEYDOWN ������� ������� 'F1' (������� �� switch �� ����� WndProc())
  /****************************************************************************/
   case WM_PAINT://��������� ����� � ������� ������� ������������� ���� � ��������� ��������� ��������
    static PAINTSTRUCT ps; //��������� ��� ������ BeginPaint()
    hDC = BeginPaint(hWnd,&ps);//  � ������ WM_PAINT
	SetTextColor(hDC, RGB(212, 154, 106));//���� ���� ������
        //SetBkColor(hDC, RGB(255,0,255)); //���� ���� �������� 
          SetBkMode(hDC,TRANSPARENT);      //��� �������� �� ��������
          pStr="����� � ������������ Windows-���� (������ + ������ ����� ����):";
          TextOut(hDC,10,5,pStr,strlen(pStr));//����� ������
    	  //����� ���������� GDI �������� RGB() (�� ������������ ���� RGB() ������� ���� ���������� !!!)
		  {int X=10,Y=27;
           for(int I=0;I<300;I++)//!!!� DirectDraw ��� �������� �����!!! 
		   {
			   SetPixel(hDC, (X + I), Y, RGB(255, 209, 170));//����� �����
			   SetPixel(hDC, (X + I), Y + 7, RGB(255, 209, 170));//����� �����
			   SetPixel(hDC, (X + I), Y + 14, RGB(212, 106, 106));//RED �����
			   SetPixel(hDC, (X + I), Y + 21, RGB(212, 106, 106));//RED �����
			   SetPixel(hDC, (X + I), Y + 28, RGB(136, 204, 136));//GREEN �����
			   SetPixel(hDC, (X + I), Y + 35, RGB(136, 204, 136));//GREEN �����
			   SetPixel(hDC, (X + I), Y + 42, RGB(64, 127, 127));//BLUE �����
			   SetPixel(hDC, (X + I), Y + 49, RGB(64, 127, 127));//BLUE �����
		   }
          }   
		  pStr="��� �������� ���� � ����� DirectDraw ���������� ������� ������� 'F1' (�����: Alt-F4)";
          TextOut(hDC,10,80,pStr,strlen(pStr));//����� ������
		  pStr="��� ������ DirectDraw ������ ��������� 'F1' ����� ���������� ��� (�����: Alt-F4)";
          TextOut(hDC,10,100,pStr,strlen(pStr));//����� ������
    EndPaint(hWnd,&ps); // !!! ����������� � ����� WM_PAINT
   break;
   /*************************************************************************/ 
   case WM_DESTROY: //�������� VRAM, RAM ����� ����������� ������ ���������
       if (pPrimarySurface!=NULL)
        {//���������� � VRAM ������ ��������� �����������(������� ����� ����)
           pPrimarySurface->Release();
           pPrimarySurface = NULL;
	}
         //����������� � RAM ��������� ������ DirectDraw (� heap-�)
           pDirectDraw7->Release();
           pDirectDraw7=NULL;
           PostQuitMessage(0);//������� ��������� WM_QUIT == ��������� ������
   break; 
   default: return(DefWindowProc(hWnd,uMsgName,wParam,lParam));
  }
  return 0;
} //����� ������� ������� WndProc()
/***************************************************************************/

//�������� ������� ������� == ����� ����� � Windows-������
int APIENTRY WinMain(HINSTANCE hInstance,HINSTANCE hPrevInstance,
                         LPSTR lpszCmdLine,int iCmdShow)
{ static char szWndClass[]="WndClass for PPKK_Min.cpp";//��� ������ ����
  static char szWndTitle[]="���������� ������ DirectDraw (��� DirectX7)"; 
/***************************************************************************/
  WNDCLASSEX wndclass;//��������� ��������� ��� ���������� ������ ����(12 �����) 
     wndclass.lpszClassName = szWndClass; //��� ������ ���� (�����, ��. ����)
     wndclass.lpfnWndProc = WndProc;      //��� ������� �������
     wndclass.style=CS_SAVEBITS|CS_VREDRAW|CS_HREDRAW|CS_DBLCLKS;//����� ����
     wndclass.hInstance = hInstance;      // ����� ����������� ���������� ������� ����������      
     wndclass.hIcon = LoadIcon( NULL, IDI_APPLICATION );// ������ ...
     wndclass.hIconSm = LoadIcon( NULL, IDI_QUESTION ); // ������ ...
     wndclass.hCursor = LoadCursor( NULL, IDC_ARROW );  // ����� �������
   //wndclass.hbrBackground =(HBRUSH) GetStockObject(BLACK_BRUSH);//������ ��� = NULL
   //  wndclass.hbrBackground =(HBRUSH)( COLOR_GRAYTEXT + 1 );//����� ��� ������� ������� ����  
	 wndclass.hbrBackground =(HBRUSH)( COLOR_WINDOW + 1 );//����� ��� ������� ������� ����
     wndclass.lpszMenuName = NULL;
     wndclass.cbClsExtra = 0;
     wndclass.cbWndExtra = 0;
     wndclass.cbSize=sizeof(wndclass);
  RegisterClassEx(&wndclass);//������  �������� �� wndclass ������� � ���������� 
/****************************************************************************/
  HWND hWnd=hWndG=  //����� (handle) ���� ���� � RAM (�������� ���� � RAM (��� ������ �� �����)
     CreateWindowEx(0, //WS_EX_TOPMOST, //����� ���� (��������������)
                    szWndClass,         //��� ������ ���� (�����, ��. ����)
	                szWndTitle,	        //����� ��������� � ���� (�����, ��. ����)
                    WS_POPUP,           //����� ���� ==  ��� ��������� ���������� ����
                                        //������� ������� ���� == ���� �����
				  //WS_OVERLAPPEDWINDOW,//����� - ������������ ����
                  //������� �������� ������� ������� ���� (��������� �����������)
                    0,0,		//���������� X � Y ����� ����� ���� ����
                    GetSystemMetrics(SM_CXSCREEN),//������ ���� � pixel
                    GetSystemMetrics(SM_CYSCREEN),//������ ���� � pixel
                //  CW_USEDEFAULT, CW_USEDEFAULT,CW_USEDEFAULT, CW_USEDEFAULT, 
                    NULL,		//���������� ������������� ����
                    NULL,		//���������� ����
                    hInstance,  //��������� (==�����) ������� ���������� ����������
                    NULL);		//�������������� ������
/****************************************************************************/
    ShowWindow(hWnd,iCmdShow); // ����������� ���� �� ������  
    UpdateWindow(hWnd);        // ������� ������� ��������� WM_PAINT
/****************************************************************************/
   MSG msg;   // ��������� ��������� ��� ���������� ���������
   //"�����������" ���� ��������� ��������� - ������� ������� �������
   while(GetMessage(&msg,NULL,0,0))
    {TranslateMessage(&msg);
     DispatchMessage(&msg);
    }//����� �� ����� ��� ��������� ��������� ������ WM_QUIT
  return msg.wParam; 
} // ����� WinMain
