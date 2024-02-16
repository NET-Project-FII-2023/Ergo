import api from "../api";
import { NotificationDto } from "./types";

export async function readNotification(notification: NotificationDto, token: string) : Promise<boolean> {
  try {
    const response = await api.put(`/api/v1/InboxItem/${notification.inboxItemId}`, {
      isRead: true,
      createdDate: notification.createdDate,
      message: notification.message,
      userId: notification.userId,
    }, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    if (response.status !== 200) {
      throw new Error(response.data.message);
    }
    return true;
  } catch (error) {
    console.error(error);
    return false;
  }
}