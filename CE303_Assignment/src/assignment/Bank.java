package assignment;

import java.util.HashMap;

public class Bank {
    final HashMap<Integer, Trader> traders = new HashMap<>();

    public void createTrader(int traderId, boolean hasStock){
        Trader trader = new Trader(traderId, hasStock);
        addToTraders(trader);
    }

    public boolean hasTraderStock(int traderId) {
        return traders.get(traderId).hasStock();
    }

    public void transfer(int fromTrader, int toTrader) throws Exception {
        synchronized (traders) {
            if (!traders.get(fromTrader).hasStock())
                throw new Exception("Trader " + fromTrader + " does not have a stock. Unable to transfer.");
            traders.get(fromTrader).setStock(false);
            traders.get(toTrader).setStock(true);
        }
    }

    public HashMap<Integer, Trader> getCurrentTraders() {
        return traders;
    }

    public void addToTraders(Trader t) {
        traders.put(t.getTraderId(), t);
    }
}
