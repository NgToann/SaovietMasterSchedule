   M S B A M L   `   `   `  ����  MasterSchedule/    )MasterSchedule.Views.InsockDeliveryWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$'  !Master-Schedule - Insock Delivery��5   	   $	��560��63   $	��700��6C    � WindowStartupLocation$ CenterScreen=�6i   .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5
   
   � -   5         �Name$ 	gridTotal��5
      $��1��6"    � ColumnDefinitions 5      �� 5      ��6#   ��  �� ElementName$ dgInsockDelivery�� �� Path$ BorderThickness.Left��� 5      ��6#   �� $ dgInsockDelivery��$ RowHeaderWidth�5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgInsockDelivery��5         
IsReadOnly . 	   RowHeaderWidth$	 5Q�6?   
   AutoGenerateColumns
 .     FrozenColumnCount$ 1��5   /    �� AlternationCount$ 2��6E      AlternatingRowBackground
 �����   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   CanUserReorderColumns .  5      5      5      