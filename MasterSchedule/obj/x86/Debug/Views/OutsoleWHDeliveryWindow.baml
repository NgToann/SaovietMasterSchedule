   M S B A M L   `   `   `  ����  MasterSchedule2    ,MasterSchedule.Views.OutsoleWHDeliveryWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$+  %Master Schedule - Outsole WH Delivery��5   	   $	��560��67   $	��700��6G    � WindowStartupLocation$ CenterScreen=�6m   .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5
   
   � -   5         �Name$ 	gridTotal��5
      $��1��6"    � ColumnDefinitions 5      �� 5      ��6#   ��  �� ElementName$ dgInventory�� �� Path$ BorderThickness.Left��� 5      ��6#   �� $ dgInventory��$ RowHeaderWidth�5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgInventory��5         
IsReadOnly . 	   RowHeaderWidth$	 5Q�6:   
   AutoGenerateColumns
 .     FrozenColumnCount$ 1��5   /    �� AlternationCount$ 2��6E      AlternatingRowBackground
 �����   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   CanUserReorderColumns .  5      5      5      