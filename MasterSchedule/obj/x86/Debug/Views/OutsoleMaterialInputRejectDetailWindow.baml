   M S B A M L   `   `   `  ����g  cMicrosoft.ReportViewer.WinForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3atSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.Reportviewer.WinFormsMicrosoft.Reporting.WinForms  5   	    MasterScheduleA   ;MasterSchedule.Views.OutsoleMaterialInputRejectDetailWindow   [rpvSclr-namespace:Microsoft.Reporting.WinForms;assembly=Microsoft.Reportviewer.WinForms  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$<  6Master-Schedule - Input Outsole Material Reject Detail��5   	    � WindowStartupLocation$ CenterScreen=�6_   $	��450��6�   $	��700��6�   $��14(�6�   .��5      � $��2q�6    � RowDefinitions 5   
   �� 5      $��* �6   .+�5
   
   � -   5         �Name$ gridShowReport��5
      $y�	Collapsed=�6'   .+�5      7  1System.Windows.Forms.Integration.WindowsFormsHost     Child. 5      /   )Microsoft.Reporting.WinForms.ReportViewer  -   5        reportViewer��   ShowFindControls .  5   9   5      5      � 5   
   -   5        $ gridInputData��5      $y�Visible=�6&    5      �� 5      $
��auto �6    �� 5      $
��auto �6    �� 5      $	��100��6    �� 5      $
��auto �6    �� 5      $
��auto �6    .+�5      �� $��0,10,0,0q�6   $��Chi Tiết Hàng Phế��6.    �� TextAlignment$ Center=�6G   $��25(�6^   $
��Bold"�6l   �� 5      $��1��6    �� Orientation$ 
Horizontal=�6*   $��0,10,0,0q�6C   .+�5      �� $ ��ProductNo (Đơn Hàng):  ��6    �� 5      -   5         ��Name$ txtProductNo��5       $
��Bold"�66   �� 5      $#��Supplier (Nhà Cung Cấp):  ��6    $��20,0,0,0q�6B   �� 5      -   5        $ txtSupplierName��5       $
��Bold"�69   �� 5      $ ��IndexNo (Công đoạn):  ��6    $��20,0,0,0q�6>   �� 5       -   5        $ 
txtIndexNo��5        $
��Bold"�64   �� 5"      $��Round (Lần kiểm):  ��6    $��20,0,0,0q�6;   �� 5#      -   5        $ txtRound��5#       $
��Bold"�62   5$      &   System.Windows.Controls.DataGrid  5%      -	   5        	  Name$#	 dgOutsoleMaterialRejectDetail��5%   9   $��0,5,0,0q�6   $��2��6,   
   AutoGenerateColumns
 .     CanUserAddRows .     FrozenColumnCount$ 1��6�      SelectionUnit$ CellOrRowHeader=�6�    �� AlternationCount$ 2��6�      AlternatingRowBackground
 �����   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����
���   �$��13(�5&   �   5'      �� 5(      $��3��6   $ 
Horizontal=�6*   $��0,2,0,0q�6C   $��Right=�6T   .+�5)      �� -
   5         ��Name$ btnSave��5)      $��13(�6.   $��_Save (Lưu)��6<   $	��120��6R   6a   5*      5+      5,      5-      