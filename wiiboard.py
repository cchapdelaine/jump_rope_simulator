from bluetooth import *
import time

DEVICE_NAME = "Nintendo RVL-WBC-01"

class EventProcessor:
    def __init__(self):
        self.measured = False
        self.done = False
        self._events = []

        def mass(self, event):
            if event.totalWeight > 30:
                self._events.append(event.totalWeight)
                if not self._measured:
                    print("Starting measurement.")
                    self._measured = True
            elif self._measured:
                self.done = True

    @property
    def weight(self):
        if not self._events:
            return 0
        histogram = collections.Counter(round(num, 1) for num in self._events)
        return histogram.most_common(1)[0][0]
        
class BoardEvent:
    def __init__(self, topLeft, topRight, bottomLeft, bottomRight, buttonPressed, buttonReleased):

        self.topLeft = topLeft
        self.topRight = topRight
        self.bottomLeft = bottomLeft
        self.bottomRight = bottomRight
        self.buttonPressed = buttonPressed
        self.buttonReleased = buttonReleased
        #convenience value
        self.totalWeight = topLeft + topRight + bottomLeft + bottomRight
        
class WiiBoard:
    def __init__(self, processor):
        self.recieve_socket = None
        self.control_socket = None
        
        self.processor = processor
        self.calibration = []
        self.calibrationRequested = False
        self.LED = False
        self.address = None
        self.buttonDown = False
 
        for i in range(3):
            self.calibration.append([])
            for j in range(4):
                self.calibration[i].append(float("inf"))  # high dummy value so events with it don't register
        
        self.status = "Disconnected"
        self.lastEvent = BoardEvent(0, 0, 0, 0, False, False)
        
        try:
            self.recieve_socket = BluetoothSocket(RFCOMM)
            self.control_socket = BluetoothSocket(RFCOMM)
        except ValueError:
            raise Exception("Error: Bluetooth not found")
            
    def isConnected(self):
        return self.status == "Connected"
        
    def connect(self, address):
        if address is None:
            print("No address given")
            return
        self.recieve_socket.connect((address, 0x13))
        self.control_socket.connect((address, 0x11))
        
        if self.recieve_socket and self.control_socket:
            print("Connected to Wii board at address " + address)
            self.status = "Connected"
            self.address = address
            self.calibrate()
            useExt = ["00", COMMAND_REGISTER, "04", "A4", "00", "40", "00"]
            self.send(useExt)
            self.setReportingType()
            print("Wiiboard connected")
        else:
            print("Could not connect to Wiiboard at address " + address)

    def recieve(self):
        while self.status == "Connected" and not self.processor.done:
            data = self.receivesocket.recv(25)
            intype = int(data.encode("hex")[2:4])
            if intype == INPUT_STATUS:
                # TODO: Status input received. It just tells us battery life really
                self.setReportingType()
            elif intype == INPUT_READ_DATA:
                if self.calibrationRequested:
                    packetLength = (int(str(data[4]).encode("hex"), 16) / 16 + 1)
                    self.parseCalibrationResponse(data[7:(7 + packetLength)])

                    if packetLength < 16:
                        self.calibrationRequested = False
            elif intype == EXTENSION_8BYTES:
                self.processor.mass(self.createBoardEvent(data[2:12]))
            else:
                print("ACK to data write received")

        self.status = "Disconnected"
        self.disconnect()
        
    def disconnect(self):
        if self.status == "Connected":
            self.status = "Disconnecting"
            while self.status == "Disconnecting":
                self.wait(100)
        try:
            self.recieve_socket.close()
        except:
            pass
        try:
            self.control_socket.close()
        except:
            pass
        print("Wii board disconnected")
        
    def discover(self):
        print("Press the sync button now: ")
        
        address = None
        nearby_devices = discover_devices(duration=5, lookup_names=True, flush_cache=True, lookup_class=False)

        print("found %d devices" % len(nearby_devices))
        for addr, name in nearby_devices:
            print("  %s - %s" % (addr, name))
            if name == DEVICE_NAME:
                address = addr
                break
                
        if address is None:
            print("No wii boards found")
        return address
    
    def createBoardEvent(self, bytes):
        buttonBytes = bytes[0:2]
        bytes = bytes[2:12]
        buttonPressed = False
        buttonReleased = False

        state = (int(buttonBytes[0].encode("hex"), 16) << 8) | int(buttonBytes[1].encode("hex"), 16)
        if state == BUTTON_DOWN_MASK:
            buttonPressed = True
            if not self.buttonDown:
                print("Button pressed")
                self.buttonDown = True

        if not buttonPressed:
            if self.lastEvent.buttonPressed:
                buttonReleased = True
                self.buttonDown = False
                print("Button released")

        rawTR = (int(bytes[0].encode("hex"), 16) << 8) + int(bytes[1].encode("hex"), 16)
        rawBR = (int(bytes[2].encode("hex"), 16) << 8) + int(bytes[3].encode("hex"), 16)
        rawTL = (int(bytes[4].encode("hex"), 16) << 8) + int(bytes[5].encode("hex"), 16)
        rawBL = (int(bytes[6].encode("hex"), 16) << 8) + int(bytes[7].encode("hex"), 16)

        topLeft = self.calc_weight(rawTL, TOP_LEFT)
        topRight = self.calc_weight(rawTR, TOP_RIGHT)
        bottomLeft = self.calc_weight(rawBL, BOTTOM_LEFT)
        bottomRight = self.calc_weight(rawBR, BOTTOM_RIGHT)
        boardEvent = BoardEvent(topLeft, topRight, bottomLeft, bottomRight, buttonPressed, buttonReleased)
        return boardEvent
        
    
    def calc_weight(self, raw, pos):
        weight = 0.0
        
        if raw < self.calibration[0][pos]:
            return weight
        elif raw < self.calibration[1][pos]:
            weight = 17 * ((raw - self.calibration[0][pos]) / float((self.calibration[1][pos] - self.calibration[0][pos])))
        elif raw > self.calibration[1][pos]:
            weight = 17 + 17 * ((raw - self.calibration[1][pos]) / float((self.calibration[2][pos] - self.calibration[1][pos])))

        return weight
        
    def get_event(self):
        return self.lastEvent
        
    def parseCalibrationResponse(self, bytes):
        index = 0
        if len(bytes) == 16:
            for i in range(2):
                for j in range(4):
                    self.calibration[i][j] = (int(bytes[index].encode("hex"), 16) << 8) + int(bytes[index + 1].encode("hex"), 16)
                    index += 2
        elif len(bytes) < 16:
            for i in xrange(4):
                self.calibration[2][i] = (int(bytes[index].encode("hex"), 16) << 8) + int(bytes[index + 1].encode("hex"), 16)
                index += 2    
     
    # Send <data> to the Wiiboard
    # <data> should be an array of strings, each string representing a single hex byte
    def send(self, data):
        if self.status != "Connected":
            return
        data[0] = "52"

        senddata = ""
        for byte in data:
            byte = str(byte)
            senddata += byte.decode("hex")

        self.controlsocket.send(senddata)
        
    #Turns the power button LED on if light is True, off if False
    #The board must be connected in order to set the light
    def setLight(self, light):
        if light:
            val = "10"
        else:
            val = "00"
            
        message = ["00", COMMAND_LIGHT, val]
        self.send(message)
        self.LED = light
     
    def calibrate(self):
        message = ["00", COMMAND_READ_REGISTER, "04", "A4", "00", "24", "00", "18"]
        self.send(message)
        self.calibrationRequested = True
        
    def setReportingType(self):
        bytearr = ["00", COMMAND_REPORTING, CONTINUOUS_REPORTING, EXTENSION_8BYTES]
        self.send(bytearr)

    def wait(self, millis):
        time.sleep(millis / 1000.0)
        
        
def main():
    processor = EventProcessor()
    board = WiiBoard(processor)
    
    print("Discovering Board...")
    address = board.discover()
    
    print("Trying to connect...")
    board.connect(address) #must be in sync mode
    board.wait(200)
    
    board.setLight(True)
    board.receive()
    
    print(processor.weight)
    
    
if __name__ == "__main__":
    main()
    
# addr = None
# print("performing inquiry...")
# device_list = bluetooth.discover_devices(duration=10, lookup_names=True)
# for device in device_list:
    # if device[1] == DEVICE_NAME:
        # addr = device[0]

# print("found %d devices" % len(device_list))        

# for addr, name in device_list:
    # try:
        # print("  %s - %s" % (addr, name))
    # except UnicodeEncodeError:
        # print("  %s - %s" % (addr, name.encode('utf-8', 'replace')))
