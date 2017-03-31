class Producer extends Thread {
  private Manage cubbyhole;
  private int number;

  public Producer(Manage c, int number) {
    this.cubbyhole = c;
    this.number = number;
  }

  @Override
  public void run() {
    for (int i = 0; i < 10; i++) {
      cubbyhole.put(i);
      System.out.println("Producer" + this.number + " -> " + i);
      try {
        sleep((int) (Math.random() * 100));
      }
      catch (InterruptedException e) {
      }
    }
  }
}