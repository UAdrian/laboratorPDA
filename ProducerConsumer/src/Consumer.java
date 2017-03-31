class Consumer extends Thread {
  private Manage cubbyhole;
  private int number;

  public Consumer(Manage c, int number) {
    cubbyhole = c;
    this.number = number;
  }

  @Override
  public void run() {
    int value = 0;
    for (int i = 0; i < 10; i++) {
      value = cubbyhole.get();
      System.out.println("Consumer" + this.number + " <- " + value);
    }
  }
}