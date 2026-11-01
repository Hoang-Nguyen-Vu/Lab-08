// Lab 08 - Delegate & Event: chương trình demo.

using System;
using System.Collections.Generic;
using System.Text;
using DeviceManagement;

Console.OutputEncoding = Encoding.UTF8;

// 1. Publisher: các thiết bị trong phòng dạy
var projector = new ProjectorRemote("PRJ-01", "Máy chiếu Epson");
var airCon = new AirConditionerRemote("AC-01", "Điều hòa Daikin", "A101");
var mic = new WirelessMicrophone("MIC-01", "Micro Shure");

var devices = new List<TeachingDevice> { projector, airCon, mic };

// 2. Subscriber: các thành phần quan tâm đến sự kiện
var notificationService = new ConsoleNotificationService();
var auditLogger = new DeviceAuditLogger();
var maintenanceService = new MaintenanceService();

// 3. Coordinator quản lý toàn bộ việc đăng ký / hủy đăng ký (+= / -=)
using (var coordinator = new DeviceMonitoringCoordinator(
           devices, notificationService, auditLogger, maintenanceService))
{
    coordinator.Subscribe();
    coordinator.Subscribe(); // gọi lần 2: không được đăng ký trùng (mỗi cảnh báo chỉ in 1 lần)

    PrintHeader("A. Đổi trạng thái (StatusChanged)");
    projector.ChangeStatus(DeviceStatus.Borrowed);
    projector.ChangeStatus(DeviceStatus.Borrowed);          // trùng -> không có event
    projector.ChangeStatus(DeviceStatus.UnderMaintenance);
    projector.ChangeStatus(DeviceStatus.Available);

    PrintHeader("B. Máy chiếu: pin 100 -> 50 -> 15 -> 10 -> 0 -> 0");
    projector.UpdateBatteryLevel(50);   // vùng an toàn, không cảnh báo
    projector.UpdateBatteryLevel(15);   // < 20%  -> Warning
    projector.UpdateBatteryLevel(10);   // cùng vùng Warning -> theo đề KHÔNG báo lại
    projector.UpdateBatteryLevel(0);    // = 0%   -> Critical
    projector.UpdateBatteryLevel(0);    // cùng vùng Critical -> theo đề KHÔNG báo lại

    PrintHeader("C. Điều hòa: phản hồi -> không phản hồi");
    airCon.UpdateResponseState(false);  // Critical
    airCon.UpdateResponseState(false);  // lặp lại -> không cảnh báo
    airCon.UpdateResponseState(true);   // phục hồi -> Information (phần mở rộng, đề không yêu cầu)

    PrintHeader("D. Micro không dây: pin & chất lượng tín hiệu");
    mic.UpdateBatteryLevel(25);         // < 30%  -> Warning
    mic.UpdateSignalQuality(35);        // < 40%  -> Warning
    mic.UpdateSignalQuality(0);         // = 0    -> Critical
    mic.UpdateBatteryLevel(0);          // = 0    -> Critical

    PrintHeader("E. Dữ liệu không hợp lệ (InvalidDeviceReadingException)");
    TryAction(() => projector.UpdateBatteryLevel(120));
    TryAction(() => projector.UpdateBatteryLevel(-5));
    TryAction(() => mic.UpdateSignalQuality(101));
    TryAction(() => mic.UpdateBatteryLevel(-1));
    TryAction(() => new ProjectorRemote("PRJ-X", "Máy chiếu lỗi", 150));
    Console.WriteLine($"Pin máy chiếu sau khi nhập sai: {projector.BatteryLevel}% (không đổi)");

    PrintHeader("F. Delegate AlertFilter + FilteredAlertSubscriber");
    var criticalOnly = new FilteredAlertSubscriber(
        alert => alert.Severity == AlertSeverity.Critical,
        alert => Console.WriteLine(
            $"CRITICAL: {alert.DeviceId} - {alert.Message}"));

    airCon.AlertRaised += criticalOnly.HandleAlert;
    mic.AlertRaised += criticalOnly.HandleAlert;

    airCon.UpdateResponseState(true);
    airCon.UpdateResponseState(false);  // Critical -> cả hai subscriber đều in
    mic.UpdateBatteryLevel(100);
    mic.UpdateBatteryLevel(10);         // Warning  -> bị lọc, chỉ ConsoleNotificationService in
    mic.UpdateBatteryLevel(0);          // Critical -> cả hai subscriber đều in

    airCon.AlertRaised -= criticalOnly.HandleAlert;
    mic.AlertRaised -= criticalOnly.HandleAlert;
    Console.WriteLine("(đã hủy đăng ký criticalOnly)");

    PrintHeader("G. Hủy đăng ký bằng Unsubscribe()");
    coordinator.Unsubscribe();
    int logsBefore = auditLogger.Entries.Count;
    int requestsBefore = maintenanceService.Requests.Count;

    projector.ChangeStatus(DeviceStatus.Borrowed);
    airCon.UpdateResponseState(true);
    airCon.UpdateResponseState(false);
    Console.WriteLine(
        $"Log mới: {auditLogger.Entries.Count - logsBefore}, " +
        $"yêu cầu bảo trì mới: {maintenanceService.Requests.Count - requestsBefore} " +
        "(đều phải bằng 0)");

    coordinator.Subscribe();            // đăng ký lại; Dispose() ở cuối khối using sẽ hủy
    PrintHeader("H. Dispose() tự gọi Unsubscribe()");
}

// Sau khi coordinator bị Dispose, không còn handler nào nhận event
int logsAfterDispose = auditLogger.Entries.Count;
projector.ChangeStatus(DeviceStatus.Available);
Console.WriteLine(
    $"Sau Dispose, log mới: {auditLogger.Entries.Count - logsAfterDispose} (phải bằng 0)");

PrintHeader("Lịch sử thay đổi (DeviceAuditLogger)");
foreach (string entry in auditLogger.Entries)
{
    Console.WriteLine(entry);
}

PrintHeader("Yêu cầu bảo trì (MaintenanceService)");
foreach (string request in maintenanceService.Requests)
{
    Console.WriteLine(request);
}

static void PrintHeader(string title)
{
    Console.WriteLine();
    Console.WriteLine($"=== {title} ===");
}

static void TryAction(Action action)
{
    try
    {
        action();
        Console.WriteLine("(không có lỗi)");
    }
    catch (InvalidDeviceReadingException ex)
    {
        Console.WriteLine(
            $"Lỗi: thiết bị {ex.DeviceId}, giá trị {ex.InvalidValue} - {ex.Message}");
    }
}