using sw.azure.messaging.Commanding.Events;

namespace sw.azure.messaging.Commanding.Listeners
{
  /// <summary>
  /// Interface for IIoTDigital Notification ActionListener
  /// </summary>
  public interface IIoTDigitalNotificationActionListener
  {
    /// <summary>
    /// Method Handler for Notifier
    /// </summary>
    /// <param name="sender"></param> Sender of the Command
    /// <param name="e"></param> Notifier Commander Event Args
    void Update(object sender, IoTDigitalMessageEventArgs e);

    } //Interface: IIoTDigitalNotificationActionListener
}