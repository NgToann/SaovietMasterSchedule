   M S B A M L   `   `   `  ����g  cMicrosoft.ReportViewer.WinForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3atSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.Reportviewer.WinFormsMicrosoft.Reporting.WinForms  5   	   I EMasterSchedule, Version=1.3.0.4, Culture=neutral, PublicKeyToken=null;"clr-namespace:MasterSchedule.ViewsMasterSchedule.Views 5   	   7   1MasterSchedule.Views.UpperAccessoriesReportWindow   5   	   [rpvSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.Reportviewer.WinForms  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:MasterSchedule.Views     5   	   -   5        $��13(�5        � WindowStartupLocation$  CenterScreen=�6(    � WindowState$ 	Maximized=�6M    � Title$0 *Master Schedule - Upper Accessories Report��5	   	   $	��450��6<   $	��800��6I   .��5
      � .+�5   
   �� -   5         ��Name$ tcMain��5      .@�5      �� ��5      �� 5      $��Inspection Report��6    $��SemiBold"�69   
���   � �� Padding$ 3q�6b   .��5      �  � RowDefinitions 5      �� 5      $
��Auto �6(   �� 5      .+�5      �  � ColumnDefinitions 5      �� 5      $��* �6/   �� 5      $
��Auto �6/   .+�5      ��  �� Orientation$ 
Horizontal=�6%   .+�5      �� -   5         ��Name$ 
btnRefresh��5   %      ButtonNoneTemplate#���   69   	 � ToolTip$	 Refresh��6u   
 � Cursor$

 Hand��6�   ��5   "   �� 5   &   $��16��6,   $��16��67   $��/Images/mini_refresh.png��6C   5      �� 5       $��2,0,0,0q�6(   $1��+UPPER ACCESSORIES INSPECTION SUMMARY REPORT��69   $��17(�6l   $��SemiBold"�6z   $��Bottom=�6�   5!      �� 5"      $��Right=�6%   $ 
Horizontal=�6A   .+�5#      �� -   5         ��Name$ cboSupplier��5#   '    �� DisplayMemberPath$
 Name��6<    �� SelectedValue$ 
SupplierId��6U   $��Center=�6p   6�   �� 5$      -   5         ��Name$ txtAccessoriesName��5$   7   $��2,0,0,0q�6&   $	��120��6S   
���   �$��Center=�6v   $��2,1,0,1q�6�   $��14(�6�   $	 Input Accessories Name��6�   �� 5%      -   5        $ 	btnFilter��5%   %   #���   68   $

 Hand��6t   ��5&   "   �� 5'   &   $��16��6,   $��16��67   $��/Images/search.png��6C   5)      5*      5+      &   System.Windows.Controls.DataGrid  5,      -   5          Name$ dgSummaryReport��5,   =   $��1��6   $��0,1,0,0q�6,      AutoGenerateColumns .     
IsReadOnly .    AlternatingRowBackground
 ����� �� AlternationCount$ 2��5-   B      RowHeaderWidth$	 NaNQ�6      HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   FrozenColumnCount$ 4��5.   e      ClipboardCopyMode$ IncludeHeader=�6{      ColumnHeaderStyle 5/      �� 50       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6$   .�51   "   ��  �� Property � �� �� Value$ Center=�6O   �� 52   "    � ��$ 14(�6=   �� 53   "    � ��
 �����6?   �� 54   "    � ��
 �   �6?   �� 55   "    � ��
 �����6@   �� 56   "    � ��$ 0.5,0.5,0.5,0.5q�6D   �� 57   "    � ��$ 5,4,5,4q�6<   58         	CellStyle 5:      �� 5;      *  $System.Windows.Controls.DataGridCell"  6$   .�5<   "   ��  � ��$ Center=�6O   �� 5=   "    � ��$ Center=�6M   �� 5>   "    � ��$ 2q�6<    �� Triggers 5?   "   P� 5@   &    P� Property �@ 
IsSelected  P� Value  . . �5A   *   ��  � ��
 �����6G   �� 5B   *    � ��
 ��iA�6G   �� 5C   *    � ��$ 0,0,0,0q�6L   5D   '   5F      5H      5I      5J      �� 5K      ��5L      � 5M       5N      �� 5O      �� 5P      �� 5Q      �� 5R      �� 5S      .+�5U      	� -   5        ! 	�Name$! radDelivery��5U   &   " 	� 	GroupName$" UpperAccessoriesReport��6;   $��Delivery Report��6^   $��SemiBold"�6x   
����  �$��Center=�6�   $��Center=�6�   6�   	� 5V      -	   5        $! 	radReject��5V   &   $" UpperAccessoriesReport��69   $��1��6\   $��Reject Report��6l   $��10,0,0,0q�6�   $��SemiBold"�6�   
���  ��$��Center=�6�   $��Center=�6�   6�   	� 5W      -
   5        $! 
radBalance��5W   &   $" UpperAccessoriesReport��6:   $��2��6]   $��Balance Report��6m   $��	10,0,10,0q�6�   $��SemiBold"�6�   
���   �$��Center=�6�   $��Center=�6�   6�   	� 5X      -   5        $! radBalanceAndReject��5X   &   $" UpperAccessoriesReport��6C   $��3��6f   $��Balance + Reject Report��6v   $��SemiBold"�6�   
���  ��$��Center=�6�   $��Center=�6�   6�   �� 5Y      -   5        $ btnRefresh1��5Y   !   $��10,0,0,0q�66   $��4��6H   #���   6X   $	 Refresh��6�   $

 Hand��6�   ��5Z      �� 5[   "   $��16��6(   $��16��63   $��/Images/mini_refresh.png��6?   5]      5^      .��5`      7  1System.Windows.Forms.Integration.WindowsFormsHost  #   Child.# 5a      /   )Microsoft.Reporting.WinForms.ReportViewer  -   5        reportViewer��$   ShowFindControls$ . 5a   =   5b      5c      5d      5e      5f      