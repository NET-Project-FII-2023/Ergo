export type InboxItem = {
  userId: string;
  message: string;
  createdDate: Date;
  isRead: boolean;
};

export type NotificationDto = {
  inboxItemId: string;
  userId: string;
  message: string;
  createdDate: Date;
  isRead: boolean;
};