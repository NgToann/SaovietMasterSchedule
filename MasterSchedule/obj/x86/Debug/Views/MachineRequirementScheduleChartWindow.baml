   M S B A M L   `   `   `  ����I  EMasterSchedule, Version=1.3.0.4, Culture=neutral, PublicKeyToken=null?$clr-namespace:MasterSchedule.CustomsMasterSchedule.Customs  5   	   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35[<clr-namespace:Microsoft.Windows.Controls;assembly=WPFToolkitMicrosoft.Windows.Controls 5   	   qGclr-namespace:Microsoft.Windows.Controls.Primitives;assembly=WPFToolkit%Microsoft.Windows.Controls.Primitives 5   	   @    :MasterSchedule.Views.MachineRequirementScheduleChartWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   6local$clr-namespace:MasterSchedule.Customs     5   	   Kmy<clr-namespace:Microsoft.Windows.Controls;assembly=WPFToolkit     5   	   clocalprimitivesGclr-namespace:Microsoft.Windows.Controls.Primitives;assembly=WPFToolkit     5   	   -   5          � Title$:  4Master Schedule - Machine Requirement Schedule Chart��5   	   $	��400��6F   $
��Auto��6S    � WindowState$ 	Maximized=�6`   .��5      � $��5,5,5,5q�6    � RowDefinitions 5	   
   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $��30 �6   �� 5      $��30 �6   .+�5   
   � $��0��6   .+�5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��Date:��6   $��Center=�6)   (  "System.Windows.Controls.DatePicker  5      -   5          Name$ 
dpDateFrom��5      $��5,0,0,0q�61      SelectedDate$ 1/6/1993X�6B   6\   �� 5      $
�� to ��6   $��Center=�6(     5      -   5        $ dpDateTo��5      $ 
11/16/1992X�6/   6K   �� 5      -   5         ��Name$ 
cboSection��5      $��5,0,0,0q�6/    �� SelectedIndex$ 0��6@   @�5      �� 5      -   5        $��C��5   9   $��Cutting��6'   6D   �� 5      -   5        $��P��5   6   $
��Prep��6'   6A   �� 5      -   5        $��S��5   8   $��Sewing��6'   6C   �� 5      -   5        $��SF��5   :   $��StockFit��6'   6F   �� 5      -	   5        $��A��5   :   $��Assembly��6'   6E   5      �� 5       -
   5         ��Name$ btnViewResult��5       $��10,0,0,0q�60   $��Create Chart��6B   $
��Bold"�6Y   6k   5"      5#      �� 5$   
   -   5        	 ��Name$	 lblTitle��5$      $(��"MACHINE REQUIREMENT SCHEDULE CHART��6&   $��20(�6P   $��1��6^   $��	0,10,0,10q�6k   
 �� TextAlignment$
 Center=�6~   �� 5%   
   -   5         ��Name$ 	scrHeader��5%      $��2��6+   $��Visible=�68   $��Hidden=�6^   .��5,      &   System.Windows.Controls.DataGrid  -   5          Name$ dgvTotal��5,         FrozenColumnCount$ 1��6)   $y�Hidden=�6?      
IsReadOnly .    CanUserAddRows .     AutoGenerateColumns .     VerticalGridLinesBrush
 �����   HorizontalGridLinesBrush
 �����   HeadersVisibility$	 Row=�5-   U      SelectionUnit$
 Cell=�6m      SelectionMode$ Single=�6�      CanUserResizeColumns . 50      51      +   %MasterSchedule.Customs.CustomDataGrid  52   
   -   5          Name$ dgMachineRequirementChart��52      $��3��6B    .     RowHeaderWidth$ 5Q�53   1    .  .  $ 1��55         CanUserReorderColumns .  
 �����
 �����   Columns 57      59      � 5:   
   -   5         �Name$ gridLine��5:      $��5��6!   $
��Auto��6.   $��0,3,0,2q�6<    � ColumnDefinitions 5;      �� 5<      �� 5=      �� 5>      �� 5?      �� 5@      �� 5A      �� 5B      �� 5C      �� 5D      �� 5E      �� 5F      �� 5G      $	��130 �6#   5I      �� 5J   
   $��4��6   $ 
Horizontal=�6"   $��0,5,0,0q�6;   .+�5K      �� -   5         ��Name$ lblMachineAvailable��5K      $��16(�61   $��Calibri)�6?   {�. $��0,3,0,0q�6e   $
��Bold"�6w   �� 5L      -   5        $	 txtMachineAvailable��5L      $��50��65   $��18(�6C   $��Calibri)�6Q   {�. $��DemiBold"�6w   $��10,0,0,0q�6�   $
 Center=�6�   
��� � � �� Padding$ 0,2,0,0q�6�   5M      5N      5O      