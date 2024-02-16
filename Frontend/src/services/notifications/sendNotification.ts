import { toast } from "react-toastify";
import api from "../api";
import { InboxItem } from "./types";

export async function sendNotification(userId: string, message: string, token: string) : Promise<InboxItem | null> {
  try {
    const response = await api.post(`/api/v1/InboxItem`, {
      userId: userId,
      message: message,
    }, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    if (response.status !== 200) {
      throw new Error(response.data.validationsErrors);
    }
    return response.data.inboxItem;
  } catch (error) {
    toast.error("User could not be notified.");
    console.error(error);
    return null;
  }
}
