   M S B A M L   `   `   `  ����I  EMasterSchedule, Version=1.3.0.4, Culture=neutral, PublicKeyToken=null?$clr-namespace:MasterSchedule.CustomsMasterSchedule.Customs  5   	   1    +MasterSchedule.Views.SockliningMasterWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   6local$clr-namespace:MasterSchedule.Customs     5   	   -   5          � Title$.  (Master Schedule - Socklining Master File��5   	   $	��640��6:   $
��Auto��6G    � WindowStartupLocation$ CenterScreen=�5   
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
 Save��6k   6z   m� 5      $��_Exit��6   5      5      �� 5   
   -   5         ��Name$ lblPopup��5      $y�	Collapsed=�6&   � 5   
   -   5         �Name$ popup��5      ��.  .��5      �� -   5         ��Name$ listView��5      ��5       �� 5!      .R�5"      � .+�5#      �� ._�5$   "   �� $
��Bold"�6G   ��6,   �� *	ProductNo+6G   5%      5&      5'      5)      5*      +   %MasterSchedule.Customs.CustomDataGrid  5+   
   -   5          Name$ dgSewingMaster��5+      +   %MasterSchedule.Customs.CustomDataGrid   KeyDown$ dgSewingMaster_KeyDown��5/         CellEditEnding$# dgSewingMaster_CellEditEnding��64      Sorting$ dgSewingMaster_Sorting��6c      MouseLeftButtonDown$( "dgSewingMaster_MouseLeftButtonDown��50         BeginningEdit$" dgSewingMaster_BeginningEdit��51   ]   !   PreviewMouseLeftButtonDown$/ )dgSewingMaster_PreviewMouseLeftButtonDown��52         	MouseMove$ dgSewingMaster_MouseMove��53         	DragLeave$ dgSewingMaster_DragLeave��68      Drop$ dgSewingMaster_Drop��54         DragOver$ dgSewingMaster_DragOver��6?      SelectedCellsChanged$) #dgSewingMaster_SelectedCellsChanged��6b   $��1��5+   7   &   System.Windows.Controls.DataGrid   AutoGenerateColumns .     CanUserAddRows .     CanUserResizeRows .     FrozenColumnCount$ 14��5,   D     �� AlternationCount$  2��6[   !   AlternatingRowBackground
! �����"   CanUserReorderColumns" .  #   SelectionUnit$# CellOrRowHeader=�5.   1   $   HorizontalGridLinesBrush
$ �����%   VerticalGridLinesBrush
% �����& J� 	AllowDrop& . ' � 	Resources' 55      *  $System.Windows.Controls.DataGridCell( �� 
TargetType) �� Triggers* P� Property+ P� Value, �� Property- �� Value��%�   '        �� 56      "(  6   ) 57      P� 58      * �@ 
IsSelected+ . . �59      �� , � ��
- ��iA�6;   �� 5:      , � ��
- �����6;   5;      5=      5>      . � ContextMenu. 5?      �� 5@      -   5        / ��Name$/ 
ctmTranfer��5@      .@�5A      m� -	   5        $
 	miTranfer��5A      $"��Tranfer to Socklining Master��62   $y�	Collapsed=�6p   m� 5B      -
   5        $
 miPrintSizeRun��5B      $'��!Print Request Form Socklining W.H��67   6b   5C      0   Columns0 5E      0  *System.Windows.Controls.DataGridTextColumn  5F      ,  &System.Windows.Controls.DataGridColumn1   Header$1 SEQUENCE��6B   2   
IsReadOnly2 . 3   
Visibility$3 	Collapsed=�6f   1  +System.Windows.Controls.DataGridBoundColumn4   Binding4 6%   �� *
Sequence+6f     5G      $1 	PROD.
NO.��6C   5   
FontWeight$
5 Bold"�6Z   2 . 6   CanUserSort6 .  4 6%   �� *	ProductNo+7   	CellStyle7 5H      �� 5I      "(  6    .�5J      �� , � ��- 6;   �� *ProductNoBackground+5K      5M        5N      -   5        
colCountry��$1 COUNTRY��5N   U   2 . 6 .  4 69   �� *	Country+6x     5O      -   5        colStyle��$1 STYLE��5O   T   2 . 6 .  4 67   �� *
ShoeName+6u     5P      $1 ART.
NO.��6C   2 . 6 .  4 6%   �� *	ArticleNo+6k     5Q      $1 	PATT.
NO.��6C   2 . 6 .  4 6%   �� *	PatternNo+6l     5R      $1 QUANTITY��6B   2 . 6 .  4 6%   �� *
Quantity+6f     5S      -   5        
colETD��$	1 EFD��5S   f   2 . 6 .  4 65   �� *ETD+8 �� StringFormat$8 dd-MMM��6�     5T      $1 Sewing
Line��6D   2 . 6 .  4 6%   �� *
SewingLine+6o     5U      $1 Sewing
Start
Date��6_   2 . 6 .  4 6%   �� *SewingStartDate+$	8 M/d��6�     5V      $1 Sewing
Balance��6G   2 . 6 .  4 6%   �� *SewingBalance+6u     5W      $1 Outsole
Start
Date��6`   2 . 6 .  4 6%   �� *OutsoleStartDate+$	8 M/d��6�     5X      $1 Outsole
Balance��6H   2 . 6 .  4 6%   �� *OutsoleBalance+6w     5Y      $1 Assy
Start
Date��6a   2 . 6 .  4 6%   �� *AssemblyStartDate+$	8 M/d��6�     5Z      $1 Assembly
Balance��6I   2 . 6 .  4 6%   �� *AssemblyBalance+6y     5[      $1 Socklining
Mat's
Arrival��6O   2 . 6 .  4 6%   �� *SockliningMatsArrival+7 5\      �� 5]      "(  6    .�5^      �� , � ��- 6;   �� *!SockliningMatsArrivalForeground+�� 5_      , � ��- 6;   �� *!SockliningMatsArrivalBackground+5`      5b        5c      -   5        colSockliningLine��$1 Socklining
Line��5c   �   2 . 6 . 4 6@   �� *SockliningLine+9 �� UpdateSourceTrigger$9 PropertyChanged=�6�     5d      $1 Socklining
Start
Date��6c   2 . 6 .  4 6%   �� *SockliningStartDate+$	8 M/d��7 5e      �� 5f      "(  6    .�5g      �� , � ��- 6;   �� *SockliningStartDateForeground+5h      5j        5k      $1 Socklining
Finish
Date��6d   2 . 6 .  4 6%   �� *SockliningFinishDate+$	8 M/d��7 5l      �� 5m      "(  6    .�5n      �� , � ��- 6;   �� * SockliningFinishDateForeground+5o      5q        5r      -   5        colSockliningQuota��$1 Socklining
Quota��5r   e   2 . 6 .  4 6A   �� *SockliningQuota+6�     5s      -   5         colSockliningActualStartDate��$"1 Socklining
Actual
Start
Date��5s   y   2 . 6 .  4 6K   �� *SockliningActualStartDate+6�     5t      -   5        !colSockliningActualFinishDate��$#1 Socklining
Actual
Finish
Date��5t   {   2 . 6 .  4 6L   �� *SockliningActualFinishDate+6�     5u      -   5        colInsoleBalance��$1 Insole
Balance��5u   a   2 . 6 .  4 6?   �� *InsoleBalance+6�     5v      -   5        colInsockBalance��$1 Insock
Balance��5v   a   2 . 6 .  4 6?   �� *InsockBalance+6�   5x      � 5y   
   $��2��6   : � ColumnDefinitions: 5z      �� 5{      $
��Auto �6#   �� 5|      $
��Auto �6#   �� 5}      $��* �6#   .+�5      �� ; �� Orientation$; 
Horizontal=�6   .+�5�      �� -   5        < ��Name$< btnCaculate��5�      $	��150��6.   $
��Left=�6=   $��	Calculate��6X   {�.  6~   �� 5�      -   5        $< 
btnRefresh��5�      $��10,0,0,0q�6-   $��4��6?      ButtonNoneTemplate#���   6O   = � ToolTip$= Refresh��6�   > � Cursor$
> Hand��6�   ��5�      �� 5�      $��16��6    $��16��6+   $��/Images/mini_refresh.png��67   5�      5�      � 5�      $��2��6   $��Right=�6#   $��0,0,2,0q�6?   .+�5�      � -   5        ? �Name$? 	prgStatus��5�      $	��140��61   $��15��6=   �� 5�      -   5        $ 	lblStatus��5�      $	��140��6/   $��15��6;   @ �� Padding$@ 2,0,0,0q�6G   $��11(�6Y   $��Center=�6g   
����  �$��Italic$�6�   5�      5�      5�      5�      