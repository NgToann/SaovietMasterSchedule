   M S B A M L   `   `   `  ����I  EMasterSchedule, Version=1.2.8.9, Culture=neutral, PublicKeyToken=null?$clr-namespace:MasterSchedule.CustomsMasterSchedule.Customs  5   	   -    'MasterSchedule.Views.SewingMasterWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   6local$clr-namespace:MasterSchedule.Customs     5   	   -   5          � Title$*  $Master Schedule - Sewing Master File��5   	   $	��640��66   $
��Auto��6C    � WindowStartupLocation$ CenterScreen=�5   
    � WindowState$ 	Maximized=�6F    J� InputBindings 5      �� 5   
   
 �� Key$ S��6    �� 	Modifiers$ Controlf�6    �� Command$
 Save��61   6B    J� CommandBindings 5
      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$
 Save��5      6Q   .��5      � 	 � RowDefinitions	 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   o� 
�������@�5      m� 5      $
��File��6   .@�5      m� -   5        
 m�Name$
 btnSave��5      $��_Save��60   {�.   m� InputGestureText$ Ctrl+S��6Q    m� Command$
 Save��6k   6z   m� 5      $��_Exit��6   5      5      �� 5   
   -   5         ��Name$ lblPopup��5      $y�	Collapsed=�6&   � 5   
   -   5         �Name$ popup��5      ��.  .��5       �� -   5         ��Name$ listView��5       ��5!      �� 5"      .R�5#      � .+�5$      �� ._�5%   "   �� $
��Bold"�6G   ��6,   �� *	ProductNo+6G   5&      5'      5(      5*      5+      +   %MasterSchedule.Customs.CustomDataGrid  5-   
   -   5          Name$ dgSewingMaster��5-      +   %MasterSchedule.Customs.CustomDataGrid   KeyDown$ dgSewingMaster_KeyDown��51         CellEditEnding$# dgSewingMaster_CellEditEnding��64      Sorting$ dgSewingMaster_Sorting��6c      MouseLeftButtonDown$( "dgSewingMaster_MouseLeftButtonDown��52         BeginningEdit$" dgSewingMaster_BeginningEdit��53   [   !   PreviewMouseLeftButtonDown$/ )dgSewingMaster_PreviewMouseLeftButtonDown��54         	MouseMove$ dgSewingMaster_MouseMove��55         	DragLeave$ dgSewingMaster_DragLeave��68      Drop$ dgSewingMaster_Drop��56         DragOver$ dgSewingMaster_DragOver��6?      SelectedCellsChanged$) #dgSewingMaster_SelectedCellsChanged��6b   $��1��5-   7   &   System.Windows.Controls.DataGrid   AutoGenerateColumns .     CanUserAddRows .     CanUserResizeRows .     FrozenColumnCount$ 14��5.   D     �� AlternationCount$  2��6[   !   AlternatingRowBackground
! �����"   CanUserReorderColumns" .  #   SelectionUnit$# CellOrRowHeader=�50   1   $   HorizontalGridLinesBrush
$ �����%   VerticalGridLinesBrush
% �����& J� 	AllowDrop& . ' � 	Resources' 57      *  $System.Windows.Controls.DataGridCell( �� 
TargetType) �� Triggers* P� Property+ P� Value, �� Property- �� Value��%�   '        �� 58      "(  6   ) 59      P� 5:      * �@ 
IsSelected+ . . �5;      �� , � ��
- ��iA�6;   �� 5<      , � ��
- �����6;   5=      5?      5@      . � ContextMenu. 5A      �� 5B      -   5        / ��Name$/ 
ctmTranfer��5B      .@�5C      m� -	   5        $
 	miTranfer��5C      $��Tranfer to Sewing Master��62   $y�	Collapsed=�6l   m� 5D      -
   5        $
 miPrintSizeRun��5D      $'��!Print Request Form Socklining W.H��67   6b   5E      0   Columns0 5G      0  *System.Windows.Controls.DataGridTextColumn  5H      ,  &System.Windows.Controls.DataGridColumn1   Header$1 SEQUENCE��6B   2   
IsReadOnly2 . 3   
Visibility$3 	Collapsed=�6f   1  +System.Windows.Controls.DataGridBoundColumn4   Binding4 6%   �� *
Sequence+6f     5I      $1 	PROD.
NO.��6C   5   
FontWeight$
5 Bold"�6Z   2 . 6   CanUserSort6 .  4 6%   �� *	ProductNo+7   	CellStyle7 5J      �� 5K      "(  6    .�5L      �� , � ��- 6;   �� *ProductNoBackground+5M      5O        5P      -   5        
colCountry��$1 COUNTRY��5P   U   2 . 6 .  4 69   �� *	Country+6x     5Q      -   5        colStyle��$1 STYLE��5Q   T   2 . 6 .  4 67   �� *
ShoeName+6u     5R      $1 ART.
NO.��6C   2 . 6 .  4 6%   �� *	ArticleNo+6k     5S      $1 	PATT.
NO.��6C   2 . 6 .  4 6%   �� *	PatternNo+6l     5T      $1 QUANTITY��6B   2 . 6 .  4 6%   �� *
Quantity+6f     5U      -   5        
colETD��$	1 EFD��5U   f   2 . 6 .  4 65   �� *ETD+8 �� StringFormat$8 dd-MMM��6�     5V      $1 Memo No.��6%   9   Width5	  /System.Windows.Controls.DataGridLengthConverter$
9 Auto	 67   2 . 4 6D   �� *MemoId+6_     5W      $1 Upper
Mat's
Arrival��6J   2 . 6 .  4 6%   �� *UpperMatsArrival+7 5X      �� 5Y      "(  6    .�5Z      �� , � ��- 6;   �� *UpperMatsArrivalForeground+�� 5[      , � ��- 6;   �� *UpperMatsArrivalBackground+5\      5^        5_      $1 Sewing
Mat's
Arrival��6K   2 . 6 .  4 6%   �� *SewingMatsArrival+7 5`      �� 5a      "(  6    .�5b      �� , � ��- 6;   �� *SewingMatsArrivalForeground+�� 5c      , � ��- 6;   �� *SewingMatsArrivalBackground+5d      5f        5g      $1 OS
Mat's
Arrival��6G   2 . 6 .  4 6%   �� *OSMatsArrival+7 5h      �� 5i      "(  6    .�5j      �� , � ��- 6;   �� *OSMatsArrivalForeground+�� 5k      , � ��- 6;   �� *OSMatsArrivalBackground+5l      5n        5o      $1 Assy
Mat's
Arrival��6M   2 . 6 .  4 6%   �� *AssemblyMatsArrival+7 5p      �� 5q      "(  6    .�5r      �� , � ��- 6;   �� *AssemblyMatsArrivalForeground+�� 5s      , � ��- 6;   �� *AssemblyMatsArrivalBackground+5t      5v        5w      -   5        colSewingLine��2 . $1 Sewing
Line��5w   �   6 . 4 6<   �� *
SewingLine+: �� UpdateSourceTrigger$: PropertyChanged=�6�     5x      -   5        colSewingPrep��$1 Sewing
Prep��5x   [   2 . 6 .  4 6<   �� *
SewingPrep+6�     5y      $1 Sewing
Start
Date��6_   2 . 6 .  4 6%   �� *SewingStartDate+$	8 M/d��7 5z      �� 5{      "(  6    .�5|      �� , � ��- 6;   �� *SewingStartDateForeground+5}      5        5�      $1 Sewing
Finish
Date��6`   2 . 6 .  4 6%   �� *SewingFinishDate+$	8 M/d��7 5�      �� 5�      "(  6    .�5�      �� , � ��- 6;   �� *SewingFinishDateForeground+5�      5�        5�      -   5        colSewingQuota��$1 Sewing
Quota��5�   ]   2 . 6 .  4 6=   �� *SewingQuota+6�     5�      -   5        colSewingActualStartDate��$1 Sewing
Planned
Start
Date��5�   q   2 . 6 .  4 6G   �� *SewingActualStartDate+6�     5�      -   5        colSewingActualFinishDate��$ 1 Sewing
Planned
Finish
Date��5�   s   2 . 6 .  4 6H   �� *SewingActualFinishDate+6�     5�      -   5         colSewingActualStartDateAuto��$1 Sewing
Actual
Start
Date��5�   y   2 . 6 .  4 6K   �� *SewingActualStartDateAuto+6�     5�      -   5        !colSewingActualFinishDateAuto��$1 Sewing
Actual
Finish
Date��5�   {   2 . 6 .  4 6L   �� *SewingActualFinishDateAuto+6�     5�      -   5        colSewingBalance��$1 Sewing
Balance��5�   a   2 . 6 .  4 6?   �� *SewingBalance+6�     5�      $1 Stockfit
Finish
Date��6\   2 . 6 .  4 6%   �� *OSFinishDate+$	8 M/d��6�     5�      $1 Stockfit
Balance��6C   2 . 6 .  4 6%   �� *	OSBalance+6s     5�      $1 Cut A
Start
Date��6]   2 . 6 .  4 6%   �� *CutAStartDate+$	8 M/d��7 5�      �� 5�      "(  6    .�5�      �� , � ��- 6;   �� *CutAStartDateForeground+5�      5�        5�      $1 Cut A
Finish
Date��6^   2 . 6 .  4 6%   �� *CutAFinishDate+$	8 M/d��6�     5�      -   5        colCutBStartDate��$1 Cut B
Start
Date��5�   a   2 . 6 .  4 6?   �� *CutBStartDate+6�     5�      -   5        colCutAQuota��$1 Cut A
Quota��5�   Y   2 . 6 .  4 6;   �� *	CutAQuota+6�     5�      -   5        colAtomCutA��$1 
Atom
Cut A��5�   W   2 . 6 .  4 6:   �� *
AtomCutA+6�     5�      -   5        colAtomCutB��$1 
Atom
Cut B��5�   W   2 . 6 .  4 6:   �� *
AtomCutB+6�     5�      -   5        colLaserCutA��$1 Laser
Cut A��5�   Y   2 . 6 .  4 6;   �� *	LaserCutA+6�     5�      -   5        colLaserCutB��$1 Laser
Cut B��5�   Y   2 . 6 .  4 6;   �� *	LaserCutB+6�     5�      -   5        colHuasenCutA��$1 Huasen
Cut A��5�   [   2 . 6 .  4 6<   �� *
HuasenCutA+6�     5�      -   5        colHuasenCutB��$1 Huasen
Cut B��5�   [   2 . 6 .  4 6<   �� *
HuasenCutB+6�     5�      -   5        colComelzCutA��$1 Comelz
Cut A��5�   [   2 . 6 .  4 6<   �� *
ComelzCutA+6�     5�      -   5        colComelzCutB��$1 Comelz
Cut B��5�   [   2 . 6 .  4 6<   �� *
ComelzCutB+6�     5�      -    5        colCutAActualStartDate��$1 Cut A
Actual
Start��5�   m   2 . 6 .  4 6E   �� *CutAActualStartDate+6�     5�      -!   5        colCutAActualFinishDate��$1 Cut A
Actual
Finish��5�   o   2 . 6 .  4 6F   �� *CutAActualFinishDate+6�     5�      -"   5        colCutABalance��$1 Cut A
Balance��5�   ]   2 . 6 .  4 6=   �� *CutABalance+6�     5�      -#   5        colH_FBalance��$1 Cutprep
Balance��5�   [   2 . 6 .  4 6<   �� *
H_FBalance+6�     5�      -$   5        colCutBBalance��$1 Cut B
Balance��5�   ]   2 . 6 .  4 6=   �� *CutBBalance+6�   5�      � 5�   
   $��2��6   ; � ColumnDefinitions; 5�      �� 5�      $
��Auto �6#   �� 5�      $
��Auto �6#   .+�5�      �� -%   5        < ��Name$< btnCaculate��5�      $	��150��6*   $
��Left=�69   $��	Calculate��6T   {�.  6z   �� 5�      -&   5        $ 	lblStatus��5�      $��1��6+   $��5,0,0,0q�6;   $��11(�6L   $��Center=�6Z   
����  �$��Italic$�6�   5�      5�      5�      