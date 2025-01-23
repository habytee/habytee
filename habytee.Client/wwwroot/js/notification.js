window.requestNotificationPermission = async function() {
    const permission = await Notification.requestPermission();
    return permission;
};
