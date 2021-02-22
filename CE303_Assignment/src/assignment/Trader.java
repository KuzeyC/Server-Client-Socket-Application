package assignment;

public class Trader {
    private final int traderId;
    private boolean hasStock;

    public Trader(int traderId, boolean hasStock) {
        this.traderId = traderId;
        this.hasStock = hasStock;
    }

    public int getTraderId() {
        return traderId;
    }

    public boolean hasStock() {
        return hasStock;
    }

    public void setStock(boolean stock) {
        hasStock = stock;
    }
}
